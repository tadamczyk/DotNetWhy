namespace DotNetWhy.Core.Services;

internal class DependencyGraphService : IDependencyGraphService
{
    private readonly IDependencyGraphProvider _dependencyGraphProvider;
    private readonly ILockFileProvider _lockFileProvider;

    public DependencyGraphService(IDependencyGraphProvider dependencyGraphProvider,
        ILockFileProvider lockFileProvider)
    {
        _dependencyGraphProvider = dependencyGraphProvider;
        _lockFileProvider = lockFileProvider;
    }

    public SolutionDependencyGraph GetDependencyGraphByPackageName(string workingDirectory, string packageName)
    {
        var dependenciesPathBuilder = new StringBuilder();
        var solutionDependencyGraph = SolutionDependencyGraph.Create(Path.GetFileName(workingDirectory));
        var workingDirectoryDependencyGraph = _dependencyGraphProvider.Get(workingDirectory);
        if (workingDirectoryDependencyGraph?.Projects is null)
        {
            return solutionDependencyGraph;
        }

        foreach (var project in workingDirectoryDependencyGraph.Projects
                     .Where(p => p.IsPackageReference()))
        {
            var projectDependencyGraph = ProjectDependencyGraph.Create(project.Name);
            var projectLockFile = _lockFileProvider.Get(project.FilePath, project.RestoreMetadata.OutputPath);
            if (projectLockFile is null)
            {
                continue;
            }

            foreach (var target in project.TargetFrameworks)
            {
                var targetDependencyGraph = TargetDependencyGraph.Create(target.TargetAlias);
                var targetLockFile = projectLockFile.Targets
                    ?.FirstOrDefault(t => t.TargetFramework.Equals(target.FrameworkName));
                if (targetLockFile is null)
                {
                    continue;
                }

                foreach (var dependency in target.Dependencies)
                {
                    dependenciesPathBuilder.Clear();

                    var library = targetLockFile.Libraries
                        ?.FirstOrDefault(l => l.Name.Equals(dependency.Name));
                    if (library is null)
                    {
                        continue;
                    }

                    CreateDependenciesPaths(library, targetLockFile, dependenciesPathBuilder, 1);
                    var dependenciesPath = dependenciesPathBuilder.ToString();
                    var dependenciesPathsByPackageName =
                        GetDependenciesPathsByPackageName(dependenciesPath, packageName);

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

    private static void CreateDependenciesPaths(LockFileTargetLibrary library,
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

    private static IEnumerable<DependenciesPath[]> GetDependenciesPathsByPackageName(string dependenciesPath,
        string packageName)
    {
        var dependenciesPathsByPackage = new List<DependenciesPath[]>();

        if (dependenciesPath?.Contains(packageName) ?? false)
        {
            var depthsAndIndexesOfPackageName =
                GetDepthsAndIndexesOfPackageName(dependenciesPath, packageName);

            foreach (var (packageDepth, packageIndex) in depthsAndIndexesOfPackageName)
            {
                var nextDependencyOpenBracketCount = dependenciesPath.Length - packageIndex < 100
                    ? dependenciesPath.Length - packageIndex - 1
                    : 100;
                var nextDependencyOpenBracketIndex = dependenciesPath.IndexOf("[",
                    packageIndex,
                    nextDependencyOpenBracketCount,
                    StringComparison.InvariantCultureIgnoreCase);

                var path = dependenciesPath
                    .Substring(packageIndex,
                        (nextDependencyOpenBracketIndex > 0
                            ? nextDependencyOpenBracketIndex
                            : dependenciesPath.Length) - packageIndex);

                var currentDepth = packageDepth;
                while (--currentDepth >= 0)
                {
                    var parentDependencyIndex = dependenciesPath.LastIndexOf($"[{currentDepth}]",
                        packageIndex - 1,
                        packageIndex,
                        StringComparison.InvariantCultureIgnoreCase);
                    var nextParentDependencyOpenBracketIndex = dependenciesPath.IndexOf("[",
                        parentDependencyIndex + 1,
                        dependenciesPath.Length - parentDependencyIndex < 100
                            ? dependenciesPath.Length - parentDependencyIndex - 1
                            : 100,
                        StringComparison.InvariantCultureIgnoreCase);

                    var parentDependency =
                        dependenciesPath
                            .Substring(parentDependencyIndex,
                                nextParentDependencyOpenBracketIndex - parentDependencyIndex)
                            .Replace(":", string.Empty);
                    path = $"{parentDependency}:{path}";
                }

                path = path
                    .Replace(Environment.NewLine, string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty);
                path = Regex.Replace(path, @"\[\d\]", string.Empty);

                var dependencies = path.Split(':');
                var dependenciesPaths =
                    dependencies
                        .Where(d => !string.IsNullOrWhiteSpace(d))
                        .Select(d => DependenciesPath.Create(d.Split("/")[0], d.Split("/")[1]))
                        .ToArray();

                if (dependenciesPaths.Length > 2 && dependenciesPaths.Last().Equals(dependenciesPaths.ElementAt(dependenciesPaths.Length - 2)))
                {
                    dependenciesPaths = dependenciesPaths.Take(dependenciesPaths.Length - 1).ToArray();
                }

                if (!dependenciesPathsByPackage.Any(d => d.SequenceEqual(dependenciesPaths)))
                {
                    dependenciesPathsByPackage.Add(dependenciesPaths);
                }
            }
        }

        return dependenciesPathsByPackage;
    }

    private static List<(int packageDepth, int packageIndex)> GetDepthsAndIndexesOfPackageName(string source,
        string value)
    {
        var result = new List<(int, int)>();

        for (var index = 0;; index += value.Length)
        {
            index = source.IndexOf(value, index, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1)
            {
                return result;
            }

            var count = index < 100 ? index : 100;
            var openDepthBracketIndex =
                source.LastIndexOf("[", index - 1, count, StringComparison.InvariantCultureIgnoreCase);
            var closeDepthBracketIndex =
                source.LastIndexOf("]", index - 1, count, StringComparison.InvariantCultureIgnoreCase);
            var packageDepth =
                int.Parse(source.Substring(openDepthBracketIndex + 1,
                    closeDepthBracketIndex - openDepthBracketIndex - 1));

            result.Add((packageDepth, index));
        }
    }
}