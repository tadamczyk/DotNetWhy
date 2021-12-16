namespace DotNetWhy.Core.Models;

public class DependenciesPath
{
    public string Name { get; }
    public string Version { get; }

    private DependenciesPath(string name, string version) =>
        (Name, Version) = (name, version);

    public static DependenciesPath Create(string name, string version) =>
        new(name, version);
}