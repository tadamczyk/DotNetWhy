namespace DotNetWhy.Core.Models;

public class DependenciesPath
{
    public string Name { get; }
    public string Version { get; }

    private DependenciesPath(string name, string version) =>
        (Name, Version) = (name, version);

    internal static DependenciesPath Create(string name, string version) =>
        new(name, version);

    public override bool Equals(object obj) =>
        obj is DependenciesPath dependenciesPath && dependenciesPath.Name == Name && dependenciesPath.Version == Version;

    public override int GetHashCode() =>
        HashCode.Combine(Name, Version);
}