namespace DotNetWhy.Core;

public sealed class Request
(
    string packageName,
    string workingDirectory = null
)
{
    public string PackageName { get; } = packageName;
    public string PackageVersion { get; init; }
    public string WorkingDirectory { get; } = workingDirectory ?? Environment.CurrentDirectory;
}