namespace DotNetWhy.Domain.Commands;

internal record struct GenerateRestoreGraphFileCommand(
        string WorkingDirectory,
        string RestoreGraphOutputPath)
    : ICommand;

internal sealed class GenerateRestoreGraphFileCommandHandler
    : ICommandHandler<GenerateRestoreGraphFileCommand>
{
    public Task HandleAsync(GenerateRestoreGraphFileCommand command)
    {
        var commandResult = GetCommandResult(command.WorkingDirectory, command.RestoreGraphOutputPath);

        if (commandResult.IsSuccess) return Task.CompletedTask;

        throw new GenerateRestoreGraphFileFailedException(command.WorkingDirectory);
    }

    private static CommandResult GetCommandResult(
        string workingDirectory,
        string restoreGraphOutputPath) =>
        Command
            .Create()
            .WithArguments(GetCommandArguments(workingDirectory, restoreGraphOutputPath))
            .WithWorkingDirectory(GetCommandWorkingDirectory(workingDirectory))
            .Execute();

    private static IEnumerable<string> GetCommandArguments(
        string workingDirectory,
        string restoreGraphOutputPath) =>
        new[]
        {
            "msbuild",
            $"\"{workingDirectory}\"",
            "/t:GenerateRestoreGraphFile",
            $"/p:RestoreGraphOutputPath=\"{restoreGraphOutputPath}\""
        };

    private static string GetCommandWorkingDirectory(string workingDirectory) =>
        Path.GetDirectoryName(workingDirectory);
}