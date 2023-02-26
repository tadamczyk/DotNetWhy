namespace DotNetWhy.Services.Models;

internal sealed class Parameters : IParameters
{
    private const string VersionCommandLong = "--version";
    private const string VersionCommandShort = "-v";

    public Parameters(IReadOnlyCollection<string> value)
    {
        PackageName = value?.FirstOrDefault();

        PackageVersion =
            (value?.Contains(VersionCommandLong) ?? false) || (value?.Contains(VersionCommandShort) ?? false)
                ? value.ElementAtOrDefault(2)
                : null;

        WorkingDirectory = Environment.CurrentDirectory;
    }

    public string PackageName { get; }
    public string PackageVersion { get; }
    public string WorkingDirectory { get; }
}