namespace DotNetWhy.Domain.Models;

public record Solution(string Name)
{
    private readonly ConcurrentBag<Project> _projects = new();

    public IReadOnlyCollection<Project> Projects => _projects.OrderBy(project => project.Name).ToList().AsReadOnly();

    public int DependencyPathCounter => _projects.Sum(project => project.DependencyPathCounter);

    public bool HasProjects => _projects.Any();

    public void AddProject(Project project) => _projects.Add(project);
}