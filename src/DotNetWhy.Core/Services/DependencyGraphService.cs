namespace DotNetWhy.Core.Services;

internal class DependencyGraphService : IDependencyGraphService
{
    private readonly IDependenciesPathsProvider _dependenciesPathsProvider;
    private readonly IDependencyGraphProvider _dependencyGraphProvider;
    private readonly ILockFileProvider _lockFileProvider;

    public DependencyGraphService(
        IDependenciesPathsProvider dependenciesPathsProvider,
        IDependencyGraphProvider dependencyGraphProvider,
        ILockFileProvider lockFileProvider)
    {
        _dependenciesPathsProvider = dependenciesPathsProvider;
        _dependencyGraphProvider = dependencyGraphProvider;
        _lockFileProvider = lockFileProvider;
    }

    public SolutionDependencyGraph GetDependencyGraphByPackageName(
        string workingDirectory,
        string packageName)
    {
        var dependenciesPathBuilder = new StringBuilder();
        var solutionDependencyGraph = SolutionDependencyGraph.Create(Path.GetFileName(workingDirectory));
        var workingDirectoryDependencyGraph = _dependencyGraphProvider.Get(workingDirectory);
        if (workingDirectoryDependencyGraph?.Projects is null)
        {
            return solutionDependencyGraph;
        }

        var projectDependenciesGraphs = GetProjectDependenciesGraphs(workingDirectoryDependencyGraph);
        foreach (var projectDependencyGraph in projectDependenciesGraphs.ToList())
        {
            foreach (var target in projectDependencyGraph.TargetFrameworks)
            {
                var targetDependencyGraph = TargetDependencyGraph.Create(target.TargetAlias);
                var targetLockFile = projectDependencyGraph.LockFile.Targets?.FirstOrDefault(t => t.TargetFramework.Equals(target.FrameworkName));
                if (targetLockFile is null)
                {
                    continue;
                }

                foreach (var dependency in target.Dependencies)
                {
                    dependenciesPathBuilder.Clear();

                    var library = targetLockFile.Libraries?.FirstOrDefault(l => l.Name.Equals(dependency.Name));
                    if (library is null)
                    {
                        continue;
                    }

                    CreateDependenciesPaths(library, targetLockFile, dependenciesPathBuilder, 1);
                    var dependenciesPath = dependenciesPathBuilder.ToString();
                    var dependenciesPathsByPackageName = _dependenciesPathsProvider.GetByPackageName(dependenciesPath, packageName);

                    foreach (var dependenciesPathByPackageName in dependenciesPathsByPackageName)
                    {
                        targetDependencyGraph.AddDependenciesPath(dependenciesPathByPackageName);
                    }
                }

                if (targetDependencyGraph.DependenciesPaths.Any())
                {
                    projectDependencyGraph.AddTargetDependencyGraph(targetDependencyGraph);
                }
            }

            if (projectDependencyGraph.TargetsDependencyGraphs.Any())
            {
                solutionDependencyGraph.AddProjectDependencyGraph(projectDependencyGraph);
            }
        }

        return solutionDependencyGraph;
    }

    private IReadOnlyCollection<ProjectDependencyGraph> GetProjectDependenciesGraphs(DependencyGraphSpec workingDirectoryDependencyGraph)
    {
        var projectDependenciesGraphs = new ConcurrentBag<ProjectDependencyGraph>();

        var projects = workingDirectoryDependencyGraph.Projects.Where(p => p.IsPackageReference());
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 0.75 * 2.0))
        };

        Parallel.ForEach(projects, parallelOptions, project =>
            {
                var projectLockFile = _lockFileProvider.Get(project.FilePath, project.RestoreMetadata.OutputPath);

                if (projectLockFile is not null)
                {
                    var projectDependencyGraph = ProjectDependencyGraph.Create(project.Name);
                    projectDependencyGraph.TargetFrameworks = project.TargetFrameworks;
                    projectDependencyGraph.LockFile = projectLockFile;
                    projectDependenciesGraphs.Add(projectDependencyGraph);
                }
            });

        return projectDependenciesGraphs.OrderBy(p => p.Name).ToList();
    }

    private static void CreateDependenciesPaths(
        LockFileTargetLibrary library,
        LockFileTarget lockFileTargetFramework,
        StringBuilder dependenciesPathBuilder,
        int indentLevel)
    {
        if (library is not null)
        {
            dependenciesPathBuilder.AppendLine($"[{indentLevel - 1}]:{library.Name}/{library.Version}");
        }

        foreach (var childDependency in library?.Dependencies ?? new List<PackageDependency>())
        {
            var childLibrary = lockFileTargetFramework.Libraries
                .FirstOrDefault(l => l.Name.Equals(childDependency.Id));

            CreateDependenciesPaths(childLibrary, lockFileTargetFramework, dependenciesPathBuilder, indentLevel + 1);
        }
    }
}