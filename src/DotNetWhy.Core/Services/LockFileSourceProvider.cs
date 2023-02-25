namespace DotNetWhy.Core.Services;

internal class LockFileSourceProvider : ILockFileSourceProvider
{
    private const string LockFileName = "project.assets.json";

    public string Get(string outputDirectory) =>
        Path.Combine(
            outputDirectory,
            LockFileName);
}