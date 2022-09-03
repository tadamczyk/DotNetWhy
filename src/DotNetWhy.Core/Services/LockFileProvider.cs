namespace DotNetWhy.Core.Services;

internal class LockFileProvider : ILockFileProvider
{
    private const int MaxRetryLimit = 3;
    private readonly ILockFileSourceProvider _lockFileSourceProvider;
    private int _retryCounter;

    public LockFileProvider(ILockFileSourceProvider lockFileSourceProvider)
    {
        _lockFileSourceProvider = lockFileSourceProvider;
    }

    public LockFile Get(string workingDirectory, string outputDirectory)
    {
        try
        {
            var dotNetRestoreResult = DotNetRunner.Restore(workingDirectory);

            return dotNetRestoreResult.IsSuccess
                ? LockFileUtilities.GetLockFile(_lockFileSourceProvider.Get(outputDirectory), NullLogger.Instance)
                : throw new RestoreProjectFailedException(workingDirectory);
        }
        catch (Exception exception) when (exception is not RestoreProjectFailedException)
        {
            if (_retryCounter++ < MaxRetryLimit)
                return Get(workingDirectory, outputDirectory);
            
            throw new RestoreProjectFailedException(workingDirectory);
        }
    }
}