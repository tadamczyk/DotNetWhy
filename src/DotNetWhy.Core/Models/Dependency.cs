namespace DotNetWhy.Core.Models;

public record Dependency(string Name, string Version)
{
    public int DependenciesCounter { get; private set; }

    public IReadOnlyList<Dependency> Dependencies =>
        _dependencies as IReadOnlyList<Dependency>;

    private readonly IList<Dependency> _dependencies =
        new List<Dependency>();

    public void AddDependency(Dependency dependency) =>
        _dependencies.Add(dependency);

    public bool Contains(string packageName) =>
        Name.Contains(packageName, StringComparison.InvariantCultureIgnoreCase) || _dependencies.Any();

    public void SetDependenciesCounter(int value) =>
        DependenciesCounter = value;

    public override string ToString() =>
        $"{Name} ({Version})";
}

public static class DependencyExtensions
{
    public static Dependency ToDependency(this LockFileTargetLibrary lockFileTargetLibrary) =>
        new(lockFileTargetLibrary.Name, lockFileTargetLibrary.Version.ToString());
}