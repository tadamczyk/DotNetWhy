namespace DotNetWhy.Core;

public sealed class Request(string packageName)
{
    public string PackageName { get; } = packageName;
    public string PackageVersion { get; init; }
}