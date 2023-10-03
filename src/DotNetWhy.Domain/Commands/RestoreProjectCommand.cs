namespace DotNetWhy.Domain.Commands;

internal record struct RestoreProjectCommand(
        string WorkingDirectory)
    : Common.ICommand;

internal sealed class RestoreProjectCommandHandler(
        ICommandFactory commandFactory)
    : ICommandHandler<RestoreProjectCommand>
{
    public Task HandleAsync(RestoreProjectCommand command)
    {
        var commandResult = GetCommandResult(command.WorkingDirectory);

        if (commandResult.IsSuccess) return Task.CompletedTask;

        throw new RestoreProjectFailedException(command.WorkingDirectory);
    }

    private ICommandResult GetCommandResult(string workingDirectory) =>
        commandFactory
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