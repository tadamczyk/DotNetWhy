namespace DotNetWhy.Domain;

public record Target(string Name)
{
    private readonly List<Dependency> _dependencies = new();

    public IReadOnlyCollection<Dependency> Dependencies => _dependencies.AsReadOnly();

    public int DependencyPathCounter => _dependencies.Sum(d => d.DependencyPathCounter);

    public bool HasDependencies => _dependencies.Any();

    public void AddDependency(Dependency dependency) => _dependencies.Add(dependency);
}