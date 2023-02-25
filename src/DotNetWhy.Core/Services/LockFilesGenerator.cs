namespace DotNetWhy.Core.Services;

internal class LockFilesGenerator : ILockFilesGenerator
{
    private readonly RetryHelper _retry = new();

    public void Generate(string workingDirectory)
    {
        try
        {
            var dotNetRestoreResult = DotNetRunner.Restore(workingDirectory);

            if (!dotNetRestoreResult.IsSuccess) throw new RestoreProjectFailedException(workingDirectory);
        }
        catch (Exception exception) when (exception is not RestoreProjectFailedException)
        {
            if (_retry.CanTryAgain()) Generate(workingDirectory);

            throw new RestoreProjectFailedException(workingDirectory);
        }
    }
}