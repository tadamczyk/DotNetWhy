namespace DotNetWhy.Core.Commands;

internal static partial class DotNetRunner
{
    internal static DotNetResult Restore(string workingDirectory) =>
        DotNetExecutor
            .Initialize()
            .InDirectory(Path.GetDirectoryName(workingDirectory))
            .WithArguments(new[]
            {
                "restore",
                $"\"{workingDirectory}\""
            })
            .Execute() as DotNetResult;
}