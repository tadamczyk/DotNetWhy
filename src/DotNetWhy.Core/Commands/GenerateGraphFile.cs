namespace DotNetWhy.Core.Commands;

internal static partial class DotNetRunner
{
    internal static CommandResult GenerateGraphFile(
        string workingDirectory,
        string outputDirectory) =>
        Command
            .Create()
            .WithArguments(GetArguments(workingDirectory, outputDirectory))
            .WithWorkingDirectory(Path.GetDirectoryName(workingDirectory))
            .Execute();

    private static IEnumerable<string> GetArguments(
        string workingDirectory,
        string outputDirectory) =>
        new[]
        {
            "msbuild",
            $"\"{workingDirectory}\"",
            "/t:GenerateRestoreGraphFile",
            $"/p:RestoreGraphOutputPath=\"{outputDirectory}\""
        };
}