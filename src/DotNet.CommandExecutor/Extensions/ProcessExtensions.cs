namespace DotNet.CommandExecutor.Extensions;

internal static class ProcessExtensions
{
    internal static void SetProcessStartInfo(
        this Process process,
        string workingDirectory,
        string arguments) =>
        process.StartInfo = new ProcessStartInfo(ProcessConstants.Name, arguments)
        {
            CreateNoWindow = ProcessConstants.CreateNoWindow,
            RedirectStandardError = ProcessConstants.RedirectStandardError,
            RedirectStandardOutput = ProcessConstants.RedirectStandardOutput,
            UseShellExecute = ProcessConstants.UseShellExecute,
            WorkingDirectory = workingDirectory
        };
}