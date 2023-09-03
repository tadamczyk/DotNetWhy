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
        var searchParameters = new SearchParameters(
            query.PackageName,
            query.PackageVersion);

        var tree = GetTree(
            query.Name,
            query.RestoreGraphOutputPath,
            searchParameters);

        return Task.FromResult(tree);
    }

    private static DependencyTreeNode GetTree(
        string name,
        string restoreGraphOutputPath,
        SearchParameters searchParameters)
    {
        var root = DependencyTreeNode.Create(name);

        var dependencyGraphSpec = GetDependencyGraphSpec(restoreGraphOutputPath);
        if (dependencyGraphSpec is null) return root;

        root.AddNodes(
            dependencyGraphSpec
                .Projects
                .Where(project => project.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference)
                .Select(project => GetProjectTree(project, searchParameters))
                .Where(project => project.HasNodes));

        return root;
    }

    private static DependencyTreeNode GetProjectTree(
        PackageSpec packageSpec,
        SearchParameters searchParameters)
    {
        var project = DependencyTreeNode.Create(packageSpec.Name);

        var lockFile = GetLockFile(packageSpec.RestoreMetadata.OutputPath);
        if (lockFile is null) return project;

        project.AddNodes(
            packageSpec
                .TargetFrameworks
                .Select(target => GetTargetTree(target, lockFile, searchParameters))
                .Where(target => target.HasNodes));

        return project;
    }

    private static DependencyTreeNode GetTargetTree(
        TargetFrameworkInformation targetFramework,
        LockFile lockFile,
        SearchParameters searchParameters)
    {
        var target = DependencyTreeNode.Create(targetFramework.ToString());

        var lockFileTarget = GetLockFileTarget(lockFile, targetFramework.FrameworkName.ToString());
        if (lockFileTarget is null) return target;

        target.AddNodes(
            targetFramework
                .Dependencies
                .Select(dependency => GetLockFileTargetLibrary(lockFileTarget, dependency.Name))
                .Where(lockFileTargetLibrary => lockFileTargetLibrary is not null)
                .Select(lockFileTargetLibrary => GetLibraryTree(lockFileTarget, lockFileTargetLibrary, searchParameters))
                .Where(library => library.ContainsNode(searchParameters.PackageName, searchParameters.PackageVersion)));

        return target;
    }

    private static DependencyTreeNode GetLibraryTree(
        LockFileTarget lockFileTarget,
        LockFileTargetLibrary lockFileTargetLibrary,
        SearchParameters searchParameters)
    {
        var library = DependencyTreeNode.Create(lockFileTargetLibrary.Name, lockFileTargetLibrary.Version.ToString());

        library.AddNodes(
            lockFileTargetLibrary
                .Dependencies
                .Select(dependency => GetLockFileTargetLibrary(lockFileTarget, dependency.Id))
                .Where(childLockFileTargetLibrary => childLockFileTargetLibrary is not null)
                .Select(childLockFileTargetLibrary => GetLibraryTree(lockFileTarget, childLockFileTargetLibrary, searchParameters))
                .Where(package => package.ContainsNode(searchParameters.PackageName, searchParameters.PackageVersion)));

        return library;
    }

    private static DependencyGraphSpec GetDependencyGraphSpec(string restoreGraphOutputPath) =>
        DependencyGraphSpec
            .Load(restoreGraphOutputPath);

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