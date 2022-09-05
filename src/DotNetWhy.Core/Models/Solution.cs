namespace DotNetWhy.Core.Models;

public record Solution(string Name)
{
    public int DependenciesCounter =>
        _projects.Sum(p => p.DependenciesCounter);

    public IReadOnlyCollection<Project> Projects =>
        _projects as IReadOnlyCollection<Project>;

    private readonly IList<Project> _projects =
        new List<Project>();

    internal void AddProject(Project project) =>
        _projects.Add(project);

    internal void AddProjects(IEnumerable<Project> projects)
    {
        foreach (var project in projects)
        {
            _projects.Add(project);
        }
    }
}