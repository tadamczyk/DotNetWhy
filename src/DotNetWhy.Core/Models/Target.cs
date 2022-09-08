namespace DotNetWhy.Core.Models;

public record Target(string Name)
{
    private readonly IList<Dependency> _dependencies = new List<Dependency>();

    public ImmutableArray<Dependency> Dependencies => _dependencies.ToImmutableArray();

    public int DependencyCounter => _dependencies.Sum(d => d.DependencyCounter);

    public bool HasDependencies => _dependencies.Any();

    internal void AddDependency(Dependency dependency) => _dependencies.Add(dependency);
}