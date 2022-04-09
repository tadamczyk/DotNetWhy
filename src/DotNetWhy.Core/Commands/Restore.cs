namespace DotNetWhy.Core.Commands;

internal static partial class DotNetRunner
{
    internal static DotNetResult Restore(string workingDirectory) =>
        DotNetExecutor
            .Initialize()
            .InDirectory(Path.GetDirectoryName(workingDirectory))
            .WithArguments(GetRestoreArguments(workingDirectory))
            .Execute();

    private static IEnumerable<string> GetRestoreArguments(string workingDirectory) =>
        new[]
        {
            "restore",
            $"\"{workingDirectory}\""
        };
}