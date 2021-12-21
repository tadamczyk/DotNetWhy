namespace DotNetWhy.Core.Models;

public class DependenciesPath
{
    public string Name { get; }
    public string Version { get; }

    private DependenciesPath(string name, string version) =>
        (Name, Version) = (name, version);

    public static DependenciesPath Create(string name, string version) =>
        new(name, version);

    public override bool Equals(object obj) =>
        (obj as DependenciesPath)?.Name == Name && (obj as DependenciesPath)?.Version == Version;

    public override int GetHashCode() =>
        HashCode.Combine(Name, Version);
}