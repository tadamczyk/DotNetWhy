namespace DotNetWhy.Core.Models;

public class SolutionDependencyGraph
{
    private readonly List<ProjectDependencyGraph> _projectDependencyGraphs = new();
    public string Name { get; }
    public IReadOnlyCollection<ProjectDependencyGraph> ProjectsDependencyGraphs => _projectDependencyGraphs.AsReadOnly();

    private SolutionDependencyGraph(string name) =>
        Name = name;

    public static SolutionDependencyGraph Create(string name) =>
        new(name);

    public void AddProjectDependencyGraph(ProjectDependencyGraph projectDependencyGraph) =>
        _projectDependencyGraphs.Add(projectDependencyGraph);
}