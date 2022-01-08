namespace DotNetWhy.Core.Models;

public class SolutionDependencyGraph
{
    public string Name { get; }
    public IReadOnlyCollection<ProjectDependencyGraph> ProjectsDependencyGraphs => _projectDependencyGraphs.AsReadOnly();

    private readonly List<ProjectDependencyGraph> _projectDependencyGraphs = new();

    private SolutionDependencyGraph(string name) =>
        Name = name;

    internal static SolutionDependencyGraph Create(string name) =>
        new(name);

    internal void AddProjectDependencyGraph(ProjectDependencyGraph projectDependencyGraph) =>
        _projectDependencyGraphs.Add(projectDependencyGraph);
}