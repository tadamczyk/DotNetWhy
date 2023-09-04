namespace DotNetWhy.Domain;

public record struct DependencyTreeParameters(
    string WorkingDirectory,
    string PackageName,
    string PackageVersion = null);