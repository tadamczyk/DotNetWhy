namespace DotNetWhy.Domain;

public record Dependency(string Name, string Version)
{
    private readonly IList<Dependency> _dependencies = new List<Dependency>();

    public ImmutableArray<Dependency> Dependencies => _dependencies.ToImmutableArray();

    public int DependencyCounter =>
        HasDependencies ? _dependencies.Sum(d => d.DependencyCounter) : 1;

    public bool HasDependencies => _dependencies.Any();

    public void AddDependency(Dependency dependency) => _dependencies.Add(dependency);

    public bool IsOrContainsPackage(string packageName) =>
        Name.Contains(packageName, StringComparison.InvariantCultureIgnoreCase) || HasDependencies;

    public override string ToString() => $"{Name} ({Version})";
}