namespace DotNetWhy.Domain.Commands;

internal record struct RestoreProjectCommand(
        string WorkingDirectory)
    : ICommand;

internal sealed class RestoreProjectCommandHandler
    : ICommandHandler<RestoreProjectCommand>
{
    public Task HandleAsync(RestoreProjectCommand command)
    {
        var commandResult = GetCommandResult(command.WorkingDirectory);

        if (commandResult.IsSuccess) return Task.CompletedTask;

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