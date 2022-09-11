namespace DotNetWhy.Core.Services;

internal class DependencyGraphConverter : IDependencyGraphConverter
{
    private readonly ILockFileProvider _lockFileProvider;
    private string PackageName { get; set; }

    public DependencyGraphConverter(ILockFileProvider lockFileProvider)
    {
        _lockFileProvider = lockFileProvider;
    }

    public Solution Convert(
        DependencyGraphSpec dependencyGraphSpec,
        string solutionName,
        string packageName)
    {
        PackageName = packageName;
        var solution = new Solution(solutionName);
        var sourceProjects = GetSourceProjects(dependencyGraphSpec);

        Parallel.ForEach(sourceProjects, sourceProject =>
        {
            var project = new Project(sourceProject.Name);
            var sourceProjectLockFile = GetSourceProjectLockFile(sourceProject.FilePath, sourceProject.RestoreMetadata.OutputPath);
            if (sourceProjectLockFile is null) return;

            foreach (var sourceTarget in sourceProject.TargetFrameworks)
            {
                var target = new Target(sourceTarget.TargetAlias);
                var sourceTargetLockFile = GetSourceTargetLockFile(sourceProjectLockFile, sourceTarget.FrameworkName.ToString());
                if (sourceTargetLockFile is null) continue;

                foreach (var sourceDependency in sourceTarget.Dependencies)
                {
                    var sourceLibraryLockFile = GetSourceLibraryLockFile(sourceTargetLockFile, sourceDependency.Name);
                    if (sourceLibraryLockFile is null) continue;

                    var dependency = sourceLibraryLockFile.ToDependency();
                    CreateDependenciesPaths(sourceTargetLockFile, sourceLibraryLockFile, dependency);

                    if (dependency.HasDependencies || dependency.IsOrContainsPackage(PackageName)) target.AddDependency(dependency);
                }
                if (target.HasDependencies) project.AddTarget(target);
            }
            if (project.HasTargets) solution.AddProject(project);
        });

        return solution;
    }

    private IEnumerable<PackageSpec> GetSourceProjects(
        DependencyGraphSpec dependencyGraphSpec) =>
        dependencyGraphSpec.Projects.Where(p => p.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference);

    private LockFile GetSourceProjectLockFile(
        string filePath,
        string outputPath) =>
        _lockFileProvider.Get(filePath, outputPath);

    private LockFileTarget GetSourceTargetLockFile(
        LockFile sourceProjectLockFile,
        string name) =>
        sourceProjectLockFile.Targets.FirstOrDefault(t => t.TargetFramework.ToString().Equals(name));

    private LockFileTargetLibrary GetSourceLibraryLockFile(
        LockFileTarget sourceTargetLockFile,
        string name) =>
        sourceTargetLockFile.Libraries.FirstOrDefault(l => l.Name.Equals(name));

    private void CreateDependenciesPaths(
        LockFileTarget sourceTargetLockFile,
        LockFileTargetLibrary sourceLibraryLockFile,
        Dependency dependency)
    {
        if (!sourceLibraryLockFile.Dependencies.Any()) return;

        foreach (var libraryDependency in sourceLibraryLockFile.Dependencies)
        {
            var childSourceLibraryLockFile = GetSourceLibraryLockFile(sourceTargetLockFile, libraryDependency.Id);
            if (childSourceLibraryLockFile is null) continue;

            var childDependency = childSourceLibraryLockFile.ToDependency();
            CreateDependenciesPaths(sourceTargetLockFile, childSourceLibraryLockFile, childDependency);

            if (childDependency.IsOrContainsPackage(PackageName)) dependency.AddDependency(childDependency);
        }
    }
}