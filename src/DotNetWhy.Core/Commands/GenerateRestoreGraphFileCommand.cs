namespace DotNetWhy.Core.Commands;

internal sealed class GenerateRestoreGraphFileCommand
(
    string workingDirectory,
    string restoreGraphOutputPath
) : IResultHandler
{
    public Result Handle() =>
        new DotNetCliCommand(workingDirectory, GetArguments()).Handle();

    private IEnumerable<string> GetArguments() =>
        new[]
        {
            "msbuild",
            $"\"{workingDirectory}\"",
            "/t:GenerateRestoreGraphFile",
            $"/p:RestoreGraphOutputPath=\"{restoreGraphOutputPath}\""
        };
}