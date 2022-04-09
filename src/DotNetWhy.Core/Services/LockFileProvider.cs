using DotNetRunner = DotNetWhy.Core.Commands.DotNetRunner;

namespace DotNetWhy.Core.Services;

internal class LockFileProvider : ILockFileProvider
{
    private readonly ILockFileSourceProvider _lockFileSourceProvider;

    public LockFileProvider(ILockFileSourceProvider lockFileSourceProvider) =>
        _lockFileSourceProvider = lockFileSourceProvider;

    public LockFile Get(string workingDirectory, string outputDirectory)
    {
        var dotNetRestoreResult = DotNetRunner.Restore(workingDirectory);

        return dotNetRestoreResult.IsSuccess
            ? LockFileUtilities.GetLockFile(_lockFileSourceProvider.Get(outputDirectory), NullLogger.Instance)
            : throw new RestoreProjectFailedException(workingDirectory);
    }
}