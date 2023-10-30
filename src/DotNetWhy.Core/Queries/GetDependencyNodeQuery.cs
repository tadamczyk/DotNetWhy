namespace DotNetWhy.Core.Queries;

internal sealed class GetDependencyNodeQuery
(
    string restoreGraphOutputPath,
    string name,
    string packageName,
    string packageVersion = null
) : IResultHandler<DependencyNode>
{
    private const string LockFileName = "project.assets.json";

    public Result<DependencyNode> Handle()
    {
        var searchNode = new DependencyNode(packageName, packageVersion);
        var node = GetNode(searchNode);

        return Result<DependencyNode>.Success(node);
    }

    private DependencyNode GetNode(DependencyNode searchNode)
    {
        var node = new DependencyNode(name);

        var dependencyGraphSpec = GetDependencyGraphSpec(restoreGraphOutputPath);
        if (dependencyGraphSpec is null) return node;

        var projectNodes = dependencyGraphSpec
            .Projects
            .Where(IsPackage)
            .Select(project => GetProjectNode(project, searchNode));

        node.AddMatchingNodes(projectNodes, searchNode);

        return node;
    }

    private static DependencyNode GetProjectNode(
        PackageSpec project,
        DependencyNode searchNode)
    {
        var projectNode = new DependencyNode(project.Name);

        var lockFile = GetLockFile(project.RestoreMetadata.OutputPath);
        if (lockFile is null) return projectNode;

        var targetNodes = project
            .TargetFrameworks
            .Select(target => GetTargetNode(target, lockFile, searchNode));

        projectNode.AddMatchingNodes(targetNodes, searchNode);

        return projectNode;
    }

    private static DependencyNode GetTargetNode(
        TargetFrameworkInformation target,
        LockFile lockFile,
        DependencyNode searchNode)
    {
        var targetNode = new DependencyNode(target.ToString());

        var lockFileTarget = GetLockFileTarget(lockFile, target.FrameworkName.ToString());
        if (lockFileTarget is null) return targetNode;

        var libraryNodes = target
            .Dependencies
            .Select(dependency => GetLockFileTargetLibrary(lockFileTarget, dependency.Name))
            .Where(lockFileTargetLibrary => lockFileTargetLibrary is not null)
            .Select(lockFileTargetLibrary => GetLibraryNode(lockFileTarget, lockFileTargetLibrary, searchNode));

        targetNode.AddMatchingNodes(libraryNodes, searchNode);

        return targetNode;
    }

    private static DependencyNode GetLibraryNode(
        LockFileTarget lockFileTarget,
        LockFileTargetLibrary lockFileTargetLibrary,
        DependencyNode searchNode)
    {
        var libraryNode = new DependencyNode(lockFileTargetLibrary.Name, lockFileTargetLibrary.Version.ToString());

        var libraryNodes = lockFileTargetLibrary
            .Dependencies
            .Select(dependency => GetLockFileTargetLibrary(lockFileTarget, dependency.Id))
            .Where(childLockFileTargetLibrary => childLockFileTargetLibrary is not null)
            .Select(childLockFileTargetLibrary =>
                GetLibraryNode(lockFileTarget, childLockFileTargetLibrary, searchNode));

        libraryNode.AddMatchingNodes(libraryNodes, searchNode);

        return libraryNode;
    }

    private static DependencyGraphSpec GetDependencyGraphSpec(string path) =>
        DependencyGraphSpec.Load(path);

    private static LockFile GetLockFile(string path) =>
        LockFileUtilities.GetLockFile(Path.Combine(path, LockFileName), NullLogger.Instance);

    private static LockFileTarget GetLockFileTarget(
        LockFile lockFile,
        string targetName) =>
        lockFile
            .Targets
            .FirstOrDefault(target => target.TargetFramework.ToString().Equals(targetName));

    private static LockFileTargetLibrary GetLockFileTargetLibrary(
        LockFileTarget lockFileTarget,
        string libraryName) =>
        lockFileTarget
            .Libraries
            .FirstOrDefault(library => library.Name.Equals(libraryName));

    private static bool IsPackage(PackageSpec project) =>
        project.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference;
}