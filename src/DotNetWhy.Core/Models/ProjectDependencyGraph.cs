namespace DotNetWhy.Core.Models;

public class ProjectDependencyGraph
{
    public string Name { get; }
    public IReadOnlyCollection<TargetDependencyGraph> TargetsDependencyGraphs => _targetsDependencyGraphs.AsReadOnly();

    private readonly List<TargetDependencyGraph> _targetsDependencyGraphs = new();

    internal LockFile LockFile { get; set; }
    internal IEnumerable<TargetFrameworkInformation> TargetFrameworks { get; set; }

    private ProjectDependencyGraph(string name) =>
        Name = name;

    internal static ProjectDependencyGraph Create(string name) =>
        new(name);

    internal void AddTargetDependencyGraph(TargetDependencyGraph targetDependencyGraph) =>
        _targetsDependencyGraphs.Add(targetDependencyGraph);
}