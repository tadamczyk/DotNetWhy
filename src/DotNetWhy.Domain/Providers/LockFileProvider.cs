namespace DotNetWhy.Domain.Providers;

internal interface ILockFileProvider
{
    LockFile Get(string path);
}

internal sealed class LockFileProvider : ILockFileProvider
{
    public LockFile Get(string path) =>
        LockFileUtilities.GetLockFile(
            Path.Combine(
                path,
                FileNames.LockFile),
            NullLogger.Instance);
}