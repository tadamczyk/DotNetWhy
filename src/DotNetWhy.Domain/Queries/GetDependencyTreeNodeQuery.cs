namespace DotNetWhy.Domain.Queries;

internal record struct GetDependencyTreeNodeQuery(
        string RestoreGraphOutputPath,
        string Name,
        string PackageName,
        string PackageVersion = null)
    : IQuery;

internal sealed class GetDependencyTreeNodeQueryHandler
    : IQueryHandler<GetDependencyTreeNodeQuery, DependencyTreeNode>
{
    public Task<DependencyTreeNode> QueryAsync(GetDependencyTreeNodeQuery query)
    {
        var searchParameters = new SearchParameters(
            query.PackageName,
            query.PackageVersion);

        var root = DependencyTreeNode.Create(query.Name);
        var packageSpecs = GetPackageSpecs(query.RestoreGraphOutputPath);

        CreateRootTree(
            packageSpecs,
            root,
            searchParameters);

        return Task.FromResult(root);
    }

    private static void CreateRootTree(
        IEnumerable<PackageSpec> packageSpecs,
        DependencyTreeNode root,
        SearchParameters searchParameters) =>
        packageSpecs?.ForEach(packageSpec =>
        {
            var lockFile = GetLockFile(packageSpec.RestoreMetadata.OutputPath);
            if (lockFile is null) return;

            var project = DependencyTreeNode.Create(packageSpec.Name);
            CreateProjectTree(packageSpec.TargetFrameworks, lockFile, project, searchParameters);

            if (project.HasNodes) root.AddNode(project);
        });

    private static void CreateProjectTree(
        IEnumerable<TargetFrameworkInformation> targetFrameworks,
        LockFile lockFile,
        DependencyTreeNode project,
        SearchParameters searchParameters) =>
        targetFrameworks?.ForEach(targetFramework =>
        {
            var lockFileTarget = GetLockFileTarget(lockFile, targetFramework.FrameworkName.ToString());
            if (lockFileTarget is null) return;

            var target = DependencyTreeNode.Create(targetFramework.FrameworkName.ToString());
            CreateTargetTree(targetFramework.Dependencies, lockFileTarget, target, searchParameters);

            if (target.HasNodes) project.AddNode(target);
        });

    private static void CreateTargetTree(
        IEnumerable<LibraryDependency> dependencies,
        LockFileTarget lockFileTarget,
        DependencyTreeNode target,
        SearchParameters searchParameters) =>
        dependencies?.ForEach(dependency =>
        {
            var lockFileTargetLibrary = GetLockFileTargetLibrary(lockFileTarget, dependency.Name);
            if (lockFileTargetLibrary is null) return;

            var library = DependencyTreeNode.Create(lockFileTargetLibrary.Name, lockFileTargetLibrary.Version.ToString());
            CreateLibraryTree(lockFileTargetLibrary.Dependencies, lockFileTarget, library, searchParameters);

            if (library.ContainsNode(searchParameters.PackageName, searchParameters.PackageVersion)) target.AddNode(library);
        });

    private static void CreateLibraryTree(
        IEnumerable<PackageDependency> dependencies,
        LockFileTarget lockFileTarget,
        DependencyTreeNode library,
        SearchParameters searchParameters) =>
        dependencies?.ForEach(dependency =>
        {
            var childLockFileTargetLibrary = GetLockFileTargetLibrary(lockFileTarget, dependency.Id);
            if (childLockFileTargetLibrary is null) return;

            var childLibrary = DependencyTreeNode.Create(childLockFileTargetLibrary.Name, childLockFileTargetLibrary.Version.ToString());
            CreateLibraryTree(childLockFileTargetLibrary.Dependencies, lockFileTarget, childLibrary, searchParameters);

            if (childLibrary.ContainsNode(searchParameters.PackageName, searchParameters.PackageVersion)) library.AddNode(childLibrary);
        });

    private static IEnumerable<PackageSpec> GetPackageSpecs(string restoreGraphOutputPath) =>
        DependencyGraphSpec
            .Load(restoreGraphOutputPath)
            ?.Projects
            .Where(packageSpec => packageSpec.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference);

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

    private record struct SearchParameters(
        string PackageName,
        string PackageVersion);
}