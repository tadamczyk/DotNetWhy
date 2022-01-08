namespace DotNetWhy.Core.Models;

public class TargetDependencyGraph
{
    public string Name { get; }
    public IReadOnlyCollection<DependenciesPath[]> DependenciesPaths => _dependenciesPaths.AsReadOnly();

    private readonly List<DependenciesPath[]> _dependenciesPaths = new();

    private TargetDependencyGraph(string name) =>
        Name = name;

    internal static TargetDependencyGraph Create(string name) =>
        new(name);

    internal void AddDependenciesPath(DependenciesPath[] dependenciesPath) =>
        _dependenciesPaths.Add(dependenciesPath);
}