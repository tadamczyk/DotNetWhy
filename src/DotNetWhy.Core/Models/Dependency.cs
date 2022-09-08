namespace DotNetWhy.Core.Models;

public record Dependency(string Name, string Version)
{
    private readonly IList<Dependency> _dependencies = new List<Dependency>();

    public ImmutableArray<Dependency> Dependencies => _dependencies.ToImmutableArray();

    public int DependencyCounter =>
        HasDependencies ? _dependencies.Sum(d => d.DependencyCounter) : 1;

    public bool HasDependencies => _dependencies.Any();

    public override string ToString() => $"{Name} ({Version})";

    internal void AddDependency(Dependency dependency) => _dependencies.Add(dependency);

    internal bool IsOrContainsPackage(string packageName) =>
        Name.Contains(packageName, StringComparison.InvariantCultureIgnoreCase) || HasDependencies;
}

public static class DependencyExtensions
{
    public static Dependency ToDependency(this LockFileTargetLibrary lockFileTargetLibrary) =>
        new(lockFileTargetLibrary.Name, lockFileTargetLibrary.Version.ToString());
}