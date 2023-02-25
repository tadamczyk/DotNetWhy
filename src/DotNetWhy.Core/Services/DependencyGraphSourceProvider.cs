namespace DotNetWhy.Core.Services;

internal class DependencyGraphSourceProvider : IDependencyGraphSourceProvider
{
    private const string JsonFileExtension = ".json";
    private const string TempFileExtension = ".tmp";

    public string Get() =>
        Path.Combine(
            Path.GetTempPath(),
            Path.GetTempFileName().Replace(TempFileExtension, JsonFileExtension));
}