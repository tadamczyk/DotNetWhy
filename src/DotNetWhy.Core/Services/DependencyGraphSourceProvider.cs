namespace DotNetWhy.Core.Services;

internal class DependencyGraphSourceProvider : IDependencyGraphSourceProvider
{
    private const string TempFileExtension = ".tmp";
    private const string JsonFileExtension = ".json";

    public string Get() =>
        Path.Combine(Path.GetTempPath(), Path.GetTempFileName().Replace(TempFileExtension, JsonFileExtension));
}