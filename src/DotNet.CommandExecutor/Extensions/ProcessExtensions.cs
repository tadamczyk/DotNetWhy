namespace DotNet.CommandExecutor.Extensions;

internal static class ProcessExtensions
{
    internal static void SetProcessStartInfo(this Process process, string workingDirectory, string arguments) =>
        process.StartInfo = new ProcessStartInfo(Command.Name, arguments)
        {
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            WorkingDirectory = workingDirectory
        };
}