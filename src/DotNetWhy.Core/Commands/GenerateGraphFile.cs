namespace DotNetWhy.Core.Commands;

internal static partial class DotNetRunner
{
    internal static DotNetResult GenerateGraphFile(string workingDirectory, string outputDirectory) =>
        DotNetExecutor
            .Initialize()
            .InDirectory(Path.GetDirectoryName(workingDirectory))
            .WithArguments(new[]
            {
                "msbuild",
                $"\"{workingDirectory}\"",
                "/t:GenerateRestoreGraphFile",
                $"/p:RestoreGraphOutputPath=\"{outputDirectory}\""
            })
            .Execute() as DotNetResult;
}