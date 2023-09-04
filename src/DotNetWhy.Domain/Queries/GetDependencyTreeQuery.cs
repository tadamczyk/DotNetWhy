namespace DotNetWhy.Domain.Queries;

internal record struct GetDependencyTreeQuery(
        string RestoreGraphOutputPath,
        string Name,
        string PackageName,
        string PackageVersion = null)
    : IQuery;

internal sealed class GetDependencyTreeQueryHandler
    : IQueryHandler<GetDependencyTreeQuery, DependencyTreeNode>
{
    public Task<DependencyTreeNode> QueryAsync(GetDependencyTreeQuery query)
    {
        var searchNode = new DependencyTreeNode(
            query.PackageName,
            query.PackageVersion);

        var tree = GetTree(
            query.Name,
            query.RestoreGraphOutputPath,
            searchNode);

        return Task.FromResult(tree);
    }

    private static DependencyTreeNode GetTree(
        string name,
        string restoreGraphOutputPath,
        DependencyTreeNode searchNode)
    {
        var root = new DependencyTreeNode(name);

        var dependencyGraphSpec = GetDependencyGraphSpec(restoreGraphOutputPath);
        if (dependencyGraphSpec is null) return root;

        var projectTrees = dependencyGraphSpec
            .Projects
            .Where(project => project.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference)
            .Select(project => GetProjectTree(project, searchNode));

        root.AddMatchingNodes(
            projectTrees,
            searchNode);

        return root;
    }

    private static DependencyTreeNode GetProjectTree(
        PackageSpec packageSpec,
        DependencyTreeNode searchNode)
    {
        var project = new DependencyTreeNode(packageSpec.Name);

        var lockFile = GetLockFile(packageSpec.RestoreMetadata.OutputPath);
        if (lockFile is null) return project;

        var targetTrees = packageSpec
            .TargetFrameworks
            .Select(target => GetTargetTree(target, lockFile, searchNode));

        project.AddMatchingNodes(
            targetTrees,
            searchNode);

        return project;
    }

    private static DependencyTreeNode GetTargetTree(
        TargetFrameworkInformation targetFramework,
        LockFile lockFile,
        DependencyTreeNode searchNode)
    {
        var target = new DependencyTreeNode(targetFramework.ToString());

        var lockFileTarget = GetLockFileTarget(lockFile, targetFramework.FrameworkName.ToString());
        if (lockFileTarget is null) return target;

        var libraryTrees = targetFramework
            .Dependencies
            .Select(dependency => GetLockFileTargetLibrary(lockFileTarget, dependency.Name))
            .Where(lockFileTargetLibrary => lockFileTargetLibrary is not null)
            .Select(lockFileTargetLibrary => GetLibraryTree(lockFileTarget, lockFileTargetLibrary, searchNode));

        target.AddMatchingNodes(
            libraryTrees,
            searchNode);

        return target;
    }

    private static DependencyTreeNode GetLibraryTree(
        LockFileTarget lockFileTarget,
        LockFileTargetLibrary lockFileTargetLibrary,
        DependencyTreeNode searchNode)
    {
        var library = new DependencyTreeNode(lockFileTargetLibrary.Name, lockFileTargetLibrary.Version.ToString());

        var libraryTrees = lockFileTargetLibrary
            .Dependencies
            .Select(dependency => GetLockFileTargetLibrary(lockFileTarget, dependency.Id))
            .Where(childLockFileTargetLibrary => childLockFileTargetLibrary is not null)
            .Select(childLockFileTargetLibrary => GetLibraryTree(lockFileTarget, childLockFileTargetLibrary, searchNode));

        library.AddMatchingNodes(
            libraryTrees,
            searchNode);

        return library;
    }

    private static DependencyGraphSpec GetDependencyGraphSpec(string restoreGraphOutputPath) =>
        DependencyGraphSpec.Load(restoreGraphOutputPath);

    private static LockFile GetLockFile(string lockFileDirectory) =>
        LockFileUtilities.GetLockFile(
            Path.Combine(
                lockFileDirectory,
                FileNames.LockFile),
            NullLogger.Instance);

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
}