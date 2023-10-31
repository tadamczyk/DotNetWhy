namespace DotNetWhy.Core.Commands;

internal sealed class RestoreCommand
(
    string workingDirectory
) : IResultHandler
{
    public Result Handle() =>
        new DotNetCliCommand(workingDirectory, GetArguments()).Handle();

    private IEnumerable<string> GetArguments() =>
        new[]
        {
            "restore",
            $"\"{workingDirectory}\""
        };
}