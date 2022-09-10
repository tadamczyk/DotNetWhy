﻿namespace DotNetWhy.Core.Services;

internal class LockFileProvider : ILockFileProvider
{
    private readonly ILockFileSourceProvider _sourceProvider;
    private readonly RetryHelper _retry = new();

    public LockFileProvider(ILockFileSourceProvider sourceProvider)
    {
        _sourceProvider = sourceProvider;
    }

    public LockFile Get(
        string workingDirectory,
        string outputDirectory)
    {
        var lockFileSource = _sourceProvider.Get(outputDirectory);

        try
        {
            var dotNetRestoreResult = DotNetRunner.Restore(workingDirectory);

            return dotNetRestoreResult.IsSuccess
                ? LockFileUtilities.GetLockFile(lockFileSource, NullLogger.Instance)
                : throw new RestoreProjectFailedException(workingDirectory);
        }
        catch (Exception exception) when (exception is not RestoreProjectFailedException)
        {
            return _retry.CanTryAgain()
                ? Get(workingDirectory, outputDirectory)
                : throw new RestoreProjectFailedException(workingDirectory);
        }
    }
}