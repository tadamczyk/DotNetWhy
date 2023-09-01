namespace DotNetWhy.Domain.Commands;

internal record struct GetLockFileCommand(string WorkingDirectory) : ICommand;

internal sealed class GetLockFileCommandHandler
    : ICommandHandler<GetLockFileCommand, LockFile>
{
    private const string LockFileName = "project.assets.json";

    public LockFile Handle(GetLockFileCommand command)
    {
        var lockFilePath = GetLockFilePath(command.WorkingDirectory);

        return LockFileUtilities.GetLockFile(
            lockFilePath,
            NullLogger.Instance);
    }

    private static string GetLockFilePath(string workingDirectory) =>
        Path.Combine(
            workingDirectory,
            LockFileName);
}