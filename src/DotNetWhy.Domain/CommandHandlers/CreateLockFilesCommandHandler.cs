namespace DotNetWhy.Domain.CommandHandlers;

internal sealed class CreateLockFilesCommandHandler
    : ICommandHandler<CreateLockFilesCommand>
{
    public void Handle(CreateLockFilesCommand command)
    {
        var commandResult = GetCommandResult(command.WorkingDirectory);

        if (commandResult.IsSuccess) return;

        throw new RestoreProjectFailedException(command.WorkingDirectory);
    }

    private static CommandResult GetCommandResult(string workingDirectory) =>
        Command
            .Create()
            .WithArguments(GetCommandArguments(workingDirectory))
            .WithWorkingDirectory(GetCommandWorkingDirectory(workingDirectory))
            .Execute();

    private static IEnumerable<string> GetCommandArguments(string workingDirectory) =>
        new[]
        {
            "restore",
            $"\"{workingDirectory}\""
        };

    private static string GetCommandWorkingDirectory(string workingDirectory) =>
        Path.GetDirectoryName(workingDirectory);
}