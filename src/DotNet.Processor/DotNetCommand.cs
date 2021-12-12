namespace DotNet.Processor;

public class DotNetCommand : IDotNetCommandInDirectory,
    IDotNetCommandWithArguments,
    IDotNetCommandExecute
{
    private static string _workingDirectory;
    private static string _arguments;

    private DotNetCommand() { }

    public static IDotNetCommandInDirectory Initialize() => new DotNetCommand();

    public IDotNetCommandWithArguments InDirectory(string workingDirectory)
    {
        _workingDirectory = workingDirectory;
        return this;
    }

    public IDotNetCommandExecute WithArguments(string[] arguments)
    {
        _arguments = string.Join(" ", arguments);
        return this;
    }

    public DotNetCommandResult Execute()
    {
        var dotNetProcessStartInfo = CreateDotNetProcessStartInfo();
        var dotNetProcess = new Process();

        try
        {
            dotNetProcess.StartInfo = dotNetProcessStartInfo;
            dotNetProcess.Start();

            var output = new StringBuilder();
            var readOutput = dotNetProcess.StandardError.RewriteTextAsyncTo(output);
            var errors = new StringBuilder();
            var readErrors = dotNetProcess.StandardError.RewriteTextAsyncTo(errors);
            var processExited = dotNetProcess.WaitForExit(Processes.DotNet.TimeToExit);

            if (processExited)
            {
                Task.WaitAll(readOutput, readErrors);

                return new DotNetCommandResult(output.ToString(), errors.ToString(), dotNetProcess.ExitCode);
            }

            dotNetProcess.Kill();

            return new DotNetCommandResult(output.ToString(), errors.ToString(), (int) Status.Failed);
        }
        finally
        {
            dotNetProcess.Dispose();
        }
    }

    private static ProcessStartInfo CreateDotNetProcessStartInfo() =>
        new(Processes.DotNet.Name, _arguments)
        {
            WorkingDirectory = _workingDirectory,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
}