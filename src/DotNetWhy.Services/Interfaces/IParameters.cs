namespace DotNetWhy.Services;

public interface IParameters
{
    string PackageName { get; }
    string PackageVersion { get; }
    string WorkingDirectory { get; }
}