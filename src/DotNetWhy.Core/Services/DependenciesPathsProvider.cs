namespace DotNetWhy.Core.Services;

internal class DependenciesPathsProvider : IDependenciesPathsProvider
{
    private const int MaximumSearchLength = 100;
    private const string OpenBracket = "[";
    private const string CloseBracket = "]";
    private const string Separator = ":";
    
    public IReadOnlyCollection<DependenciesPath[]> GetByPackageName(string dependenciesPath, string packageName)
    {
        var dependenciesPathsByPackageName = new List<DependenciesPath[]>();

        if (!dependenciesPath?.Contains(packageName) ?? true)
        {
            return dependenciesPathsByPackageName;
        }

        var packageAppearances = GetPackageAppearances(dependenciesPath, packageName);
        foreach (var (packageDepth, packageIndex) in packageAppearances)
        {
            var dependencyPath = GetDependencyPath(dependenciesPath, packageIndex);
            var currentDepth = packageDepth;

            while (--currentDepth >= 0)
            {
                var parentDependency = GetParentDependency(dependenciesPath, currentDepth, packageIndex);
                dependencyPath = $"{parentDependency}{Separator}{dependencyPath}";
            }

            var dependenciesPaths = GetDependenciesPaths(dependencyPath);
            if (!dependenciesPathsByPackageName.Any(d => d.SequenceEqual(dependenciesPaths)))
            {
                dependenciesPathsByPackageName.Add(dependenciesPaths);
            }
        }

        return dependenciesPathsByPackageName;
    }

    private static List<(int packageDepth, int packageIndex)> GetPackageAppearances(string dependenciesPath, string packageName)
    {
        var packageAppearances = new List<(int, int)>();

        for (var index = 0;; index += packageName.Length)
        {
            index = dependenciesPath.IndexOf(packageName, index, StringComparison.InvariantCultureIgnoreCase);
            if (index == -1)
            {
                return packageAppearances;
            }

            var count = index < MaximumSearchLength ? index : MaximumSearchLength;
            var openDepthBracketIndex =
                dependenciesPath.LastIndexOf(OpenBracket, index - 1, count, StringComparison.InvariantCultureIgnoreCase);
            var closeDepthBracketIndex =
                dependenciesPath.LastIndexOf(CloseBracket, index - 1, count, StringComparison.InvariantCultureIgnoreCase);
            var depth =
                int.Parse(dependenciesPath.Substring(openDepthBracketIndex + 1, closeDepthBracketIndex - openDepthBracketIndex - 1));

            packageAppearances.Add((depth, index));
        }
    }

    private static string GetDependencyPath(string dependenciesPath, int packageIndex)
    {
        var nextDependencyOpenBracketCount = dependenciesPath.Length - packageIndex < MaximumSearchLength
            ? dependenciesPath.Length - packageIndex - 1
            : MaximumSearchLength;
        var nextDependencyOpenBracketIndex = dependenciesPath.IndexOf(OpenBracket,
            packageIndex,
            nextDependencyOpenBracketCount,
            StringComparison.InvariantCultureIgnoreCase);
        var dependencyPath = dependenciesPath.Substring(packageIndex,
            (nextDependencyOpenBracketIndex > 0
                ? nextDependencyOpenBracketIndex
                : dependenciesPath.Length) - packageIndex);

        return dependencyPath;
    }

    private static string GetParentDependency(string dependenciesPath, int currentDepth, int packageIndex)
    {
        var parentDependencyIndex = dependenciesPath.LastIndexOf($"{OpenBracket}{currentDepth}{CloseBracket}",
            packageIndex - 1,
            packageIndex,
            StringComparison.InvariantCultureIgnoreCase);
        var nextParentDependencyOpenBracketIndex = dependenciesPath.IndexOf(OpenBracket,
            parentDependencyIndex + 1,
            dependenciesPath.Length - parentDependencyIndex < MaximumSearchLength
                ? dependenciesPath.Length - parentDependencyIndex - 1
                : MaximumSearchLength,
            StringComparison.InvariantCultureIgnoreCase);
        var parentDependency = dependenciesPath.Substring(parentDependencyIndex,
                nextParentDependencyOpenBracketIndex - parentDependencyIndex)
            .Replace(Separator, string.Empty);

        return parentDependency;
    }

    private static DependenciesPath[] GetDependenciesPaths(string path)
    {
        var dependencies = Regex.Replace(path, @"\[\d\]", string.Empty)
            .Replace(Environment.NewLine, string.Empty)
            .Replace("\n", string.Empty)
            .Replace("\r", string.Empty)
            .Split(Separator);

        var dependenciesPaths = dependencies
            .Where(d => !string.IsNullOrWhiteSpace(d))
            .Select(d => new {Name = d.Split("/")[0], Version = d.Split("/")[1]})
            .Select(d => DependenciesPath.Create(d.Name, d.Version))
            .ToArray();

        if (dependenciesPaths.Length > 2 && dependenciesPaths.Last()
                .Equals(dependenciesPaths.ElementAt(dependenciesPaths.Length - 2)))
        {
            dependenciesPaths = dependenciesPaths.Take(dependenciesPaths.Length - 1).ToArray();
        }

        return dependenciesPaths;
    }
}