namespace DotNetWhy.Core.Models;

public record Project(string Name)
{
    public int DependenciesCounter =>
        _targets.Sum(t => t.DependenciesCounter);

    public IReadOnlyCollection<Target> Targets =>
        _targets as IReadOnlyCollection<Target>;

    private readonly IList<Target> _targets =
        new List<Target>();

    internal void AddTarget(Target target) =>
        _targets.Add(target);
}