namespace DotNetWhy.Domain.Queries;

internal record struct GetDependencyTreeQuery(
        string RestoreGraphOutputPath,
        string Name,
        string PackageName,
        string PackageVersion = null)
    : IQuery;

internal sealed class GetDependencyTreeQueryHandler(
        IDependencyGraphSpecProvider dependencyGraphSpecProvider,
        ILockFileProvider lockFileProvider,
        ILockFileTargetProvider lockFileTargetProvider,
        ILockFileTargetLibraryProvider lockFileTargetLibraryProvider)
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

    private DependencyTreeNode GetTree(
        string name,
        string restoreGraphOutputPath,
        DependencyTreeNode searchNode)
    {
        var root = new DependencyTreeNode(name);

        var dependencyGraphSpec = dependencyGraphSpecProvider.Get(restoreGraphOutputPath);
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

    private DependencyTreeNode GetProjectTree(
        PackageSpec packageSpec,
        DependencyTreeNode searchNode)
    {
        var project = new DependencyTreeNode(packageSpec.Name);

        var lockFile = lockFileProvider.Get(packageSpec.RestoreMetadata.OutputPath);
        if (lockFile is null) return project;

        var targetTrees = packageSpec
            .TargetFrameworks
            .Select(target => GetTargetTree(target, lockFile, searchNode));

        project.AddMatchingNodes(
            targetTrees,
            searchNode);

        return project;
    }

    private DependencyTreeNode GetTargetTree(
        TargetFrameworkInformation targetFramework,
        LockFile lockFile,
        DependencyTreeNode searchNode)
    {
        var target = new DependencyTreeNode(targetFramework.ToString());

        var lockFileTarget = lockFileTargetProvider.Get(lockFile, targetFramework.FrameworkName.ToString());
        if (lockFileTarget is null) return target;

        var libraryTrees = targetFramework
            .Dependencies
            .Select(dependency => lockFileTargetLibraryProvider.Get(lockFileTarget, dependency.Name))
            .Where(lockFileTargetLibrary => lockFileTargetLibrary is not null)
            .Select(lockFileTargetLibrary => GetLibraryTree(lockFileTarget, lockFileTargetLibrary, searchNode));

        target.AddMatchingNodes(
            libraryTrees,
            searchNode);

        return target;
    }

    private DependencyTreeNode GetLibraryTree(
        LockFileTarget lockFileTarget,
        LockFileTargetLibrary lockFileTargetLibrary,
        DependencyTreeNode searchNode)
    {
        var library = new DependencyTreeNode(lockFileTargetLibrary.Name, lockFileTargetLibrary.Version.ToString());

        var libraryTrees = lockFileTargetLibrary
            .Dependencies
            .Select(dependency => lockFileTargetLibraryProvider.Get(lockFileTarget, dependency.Id))
            .Where(childLockFileTargetLibrary => childLockFileTargetLibrary is not null)
            .Select(childLockFileTargetLibrary => GetLibraryTree(lockFileTarget, childLockFileTargetLibrary, searchNode));

        library.AddMatchingNodes(
            libraryTrees,
            searchNode);

        return library;
    }
}