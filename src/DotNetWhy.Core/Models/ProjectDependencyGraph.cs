namespace DotNetWhy.Core.Models;

public class ProjectDependencyGraph
{
    private readonly List<TargetDependencyGraph> _targetsDependencyGraphs = new();
    public string Name { get; }
    public IReadOnlyCollection<TargetDependencyGraph> TargetsDependencyGraphs => _targetsDependencyGraphs.AsReadOnly();

    private ProjectDependencyGraph(string name) =>
        Name = name;

    public static ProjectDependencyGraph Create(string name) =>
        new(name);

    public void AddTargetDependencyGraph(TargetDependencyGraph targetDependencyGraph) =>
        _targetsDependencyGraphs.Add(targetDependencyGraph);
}