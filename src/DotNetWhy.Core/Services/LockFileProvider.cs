namespace DotNetWhy.Core.Services;

internal class LockFileProvider : ILockFileProvider
{
    private readonly ILockFileSourceProvider _sourceProvider;

    public LockFileProvider(ILockFileSourceProvider sourceProvider) => _sourceProvider = sourceProvider;

    public LockFile Get(string outputDirectory)
    {
        var lockFileSource = _sourceProvider.Get(outputDirectory);

        return LockFileUtilities.GetLockFile(lockFileSource, NullLogger.Instance);
    }
}