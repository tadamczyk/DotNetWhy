namespace DotNetWhy.Core.Commands;

internal static partial class DotNetRunner
{
    internal static CommandResult Restore(string workingDirectory) =>
        Command
            .Create()
            .WithArguments(GetArguments(workingDirectory))
            .WithWorkingDirectory(Path.GetDirectoryName(workingDirectory))
            .Execute();

    private static IEnumerable<string> GetArguments(string workingDirectory) =>
        new[]
        {
            "restore",
            $"\"{workingDirectory}\""
        };
}