namespace DotNetWhy.Core.Commands;

internal static partial class DotNetRunner
{
    internal static DotNetResult GenerateGraphFile(
        string workingDirectory,
        string outputDirectory) =>
        DotNetExecutor
            .Initialize()
            .InDirectory(Path.GetDirectoryName(workingDirectory))
            .WithArguments(GetGenerateGraphFileArguments(workingDirectory, outputDirectory))
            .AndExecute();

    private static IEnumerable<string> GetGenerateGraphFileArguments(
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