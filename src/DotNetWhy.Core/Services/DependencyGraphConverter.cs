namespace DotNetWhy.Core.Services;

internal class DependencyGraphConverter : IDependencyGraphConverter
{
    private readonly ILockFileProvider _lockFileProvider;

    public DependencyGraphConverter(ILockFileProvider lockFileProvider)
    {
        _lockFileProvider = lockFileProvider;
    }

    public Solution ToSolution(DependencyGraphSpec dependencyGraphSpec, string solutionName, string packageName)
    {
        var solution = new Solution(solutionName);
        var projectsBag = new ConcurrentBag<Project>();
        var sourceProjects = GetSourceProjects(dependencyGraphSpec);

        Parallel.ForEach(sourceProjects, sourceProject =>
        {
            var project = new Project(sourceProject.Name);
            var sourceProjectLockFile = _lockFileProvider.Get(sourceProject.FilePath, sourceProject.RestoreMetadata.OutputPath);
            if (sourceProjectLockFile is null) return;

            foreach (var sourceTarget in sourceProject.TargetFrameworks)
            {
                var target = new Target(sourceTarget.TargetAlias);
                var sourceTargetLockFile = sourceProjectLockFile.Targets.FirstOrDefault(t => t.TargetFramework.Equals(sourceTarget.FrameworkName));
                if (sourceTargetLockFile is null) continue;

                foreach (var sourceDependency in sourceTarget.Dependencies)
                {
                    var library = sourceTargetLockFile.Libraries.FirstOrDefault(l => l.Name.Equals(sourceDependency.Name));
                    if (library is null) continue;

                    var dependency = library.ToDependency();
                    CreateDependenciesPaths(sourceTargetLockFile, library, dependency, packageName);
                    dependency.SetDependenciesCounter(0);
                    if (dependency.Dependencies.Any()) target.AddDependency(dependency);
                }
                if (target.Dependencies.Any()) project.AddTarget(target);
            }
            if (project.Targets.Any()) projectsBag.Add(project);
        });

        if (projectsBag.Any()) solution.AddProjects(projectsBag.OrderBy(p => p.Name).ToList());

        return solution;
    }

    private static IEnumerable<PackageSpec> GetSourceProjects(DependencyGraphSpec dependencyGraphSpec) =>
        dependencyGraphSpec.Projects.Where(p => p.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference);

    private static void CreateDependenciesPaths(
        LockFileTarget lockFileTarget,
        LockFileTargetLibrary library,
        Dependency dependency,
        string packageName)
    {
        if (!library.Dependencies.Any())
            return;

        foreach (var libraryDependency in library.Dependencies)
        {
            var childLibrary = lockFileTarget.Libraries.FirstOrDefault(l => l.Name.Equals(libraryDependency.Id));
            if (childLibrary is null) continue;

            var childDependency = childLibrary.ToDependency();
            
            CreateDependenciesPaths(lockFileTarget, childLibrary, childDependency, packageName);

            if (childDependency.Contains(packageName))
                dependency.AddDependency(childDependency);
        }
    }
}