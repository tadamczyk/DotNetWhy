namespace DotNetWhy.Domain;

public record Solution(string Name)
{
    private readonly ConcurrentBag<Project> _projects = new();

    public ImmutableArray<Project> Projects => _projects.OrderBy(project => project.Name).ToImmutableArray();

    public int DependencyCounter => _projects.Sum(project => project.DependencyCounter);

    public bool HasProjects => _projects.Any();

    public void AddProject(Project project) => _projects.Add(project);
}