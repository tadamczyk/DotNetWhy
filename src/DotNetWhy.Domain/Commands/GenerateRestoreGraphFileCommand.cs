namespace DotNetWhy.Domain.Commands;

internal record struct GenerateRestoreGraphFileCommand(
        string WorkingDirectory,
        string RestoreGraphOutputPath)
    : Common.ICommand;

internal sealed class GenerateRestoreGraphFileCommandHandler(
        ICommandFactory commandFactory)
    : ICommandHandler<GenerateRestoreGraphFileCommand>
{
    public Task HandleAsync(GenerateRestoreGraphFileCommand command)
    {
        var commandResult = GetCommandResult(command.WorkingDirectory, command.RestoreGraphOutputPath);

        if (commandResult.IsSuccess) return Task.CompletedTask;

        throw new GenerateRestoreGraphFileFailedException(command.WorkingDirectory);
    }

    private ICommandResult GetCommandResult(
        string workingDirectory,
        string restoreGraphOutputPath) =>
        commandFactory
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