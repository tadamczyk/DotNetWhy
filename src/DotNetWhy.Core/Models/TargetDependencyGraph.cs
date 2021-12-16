namespace DotNetWhy.Core.Models;

public class TargetDependencyGraph
{
    private readonly List<DependenciesPath[]> _dependenciesPaths = new();
    public string Name { get; }
    public IReadOnlyCollection<DependenciesPath[]> DependenciesPaths => _dependenciesPaths.AsReadOnly();

    private TargetDependencyGraph(string name) =>
        Name = name;

    public static TargetDependencyGraph Create(string name) =>
        new(name);

    public void AddDependenciesPath(DependenciesPath[] dependenciesPath) =>
        _dependenciesPaths.Add(dependenciesPath);
}