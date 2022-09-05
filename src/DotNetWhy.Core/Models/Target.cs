namespace DotNetWhy.Core.Models;

public record Target(string Name)
{
    public int DependenciesCounter =>
        _dependencies.Sum(d => d.DependenciesCounter);

    public IReadOnlyCollection<Dependency> Dependencies =>
        _dependencies as IReadOnlyCollection<Dependency>;

    private readonly IList<Dependency> _dependencies =
        new List<Dependency>();

    internal void AddDependency(Dependency dependency) =>
        _dependencies.Add(dependency);
}