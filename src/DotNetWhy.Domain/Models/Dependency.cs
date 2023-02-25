namespace DotNetWhy.Domain;

public record Dependency(string Name, string Version)
{
    private readonly List<Dependency> _dependencies = new();

    public IReadOnlyCollection<Dependency> Dependencies => _dependencies.AsReadOnly();

    public int DependencyPathCounter =>
        HasDependencies ? _dependencies.Sum(d => d.DependencyPathCounter) : 1;

    public bool HasDependencies => _dependencies.Any();

    public void AddDependency(Dependency dependency) => _dependencies.Add(dependency);

    public bool IsOrContainsPackage(string packageName) =>
        Name.Contains(packageName, StringComparison.InvariantCultureIgnoreCase) || HasDependencies;

    public override string ToString() => $"{Name} ({Version})";
}