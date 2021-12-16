namespace DotNet.CommandExecutor.Extensions;

internal static class ProcessExtensions
{
    internal static void SetDotNetProcessStartInfo(this Process process, string workingDirectory, string arguments)
    {
        process.StartInfo = new ProcessStartInfo(CommandConstants.Name, arguments)
        {
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            WorkingDirectory = workingDirectory
        };
    }
}