namespace DotNetWhy.Domain.CommandHandlers;

internal sealed class GetDependencyGraphSpecCommandHandler
    : ICommandHandler<GetDependencyGraphSpecCommand, DependencyGraphSpec>
{
    private const string JsonFileExtension = ".json";
    private const string TempFileExtension = ".tmp";

    public DependencyGraphSpec Handle(GetDependencyGraphSpecCommand command)
    {
        var dependencyGraphSpecPath = GetDependencyGraphSpecPath();

        var commandResult = GetCommandResult(command.WorkingDirectory, dependencyGraphSpecPath);

        if (commandResult.IsSuccess) return DependencyGraphSpec.Load(dependencyGraphSpecPath);

        throw new GenerateGraphFileFailedException(command.WorkingDirectory);
    }

    private static string GetDependencyGraphSpecPath() =>
        Path.Combine(
            Path.GetTempPath(),
            Path.GetTempFileName().Replace(TempFileExtension, JsonFileExtension));

    private static CommandResult GetCommandResult(
        string workingDirectory,
        string outputDirectory) =>
        Command
            .Create()
            .WithArguments(GetCommandArguments(workingDirectory, outputDirectory))
            .WithWorkingDirectory(GetCommandWorkingDirectory(workingDirectory))
            .Execute();

    private static IEnumerable<string> GetCommandArguments(
        string workingDirectory,
        string outputDirectory) =>
        new[]
        {
            "msbuild",
            $"\"{workingDirectory}\"",
            "/t:GenerateRestoreGraphFile",
            $"/p:RestoreGraphOutputPath=\"{outputDirectory}\""
        };

    private static string GetCommandWorkingDirectory(string workingDirectory) =>
        Path.GetDirectoryName(workingDirectory);
}