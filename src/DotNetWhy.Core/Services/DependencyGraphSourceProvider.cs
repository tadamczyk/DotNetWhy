namespace DotNetWhy.Core.Services;

internal class DependencyGraphSourceProvider : IDependencyGraphSourceProvider
{
    public string Get() =>
        Path.Combine(Path.GetTempPath(), Path.GetTempFileName().Replace(".tmp", ".json"));
}