namespace DotNet.CommandExecutor;

public sealed class DotNetExecutor : ICommandDirectory,
    ICommandArgument,
    ICommandExecutor
{
    private static string _workingDirectory;
    private static string _arguments;

    private DotNetExecutor() => Expression.Empty();

    public static ICommandDirectory Initialize() => new DotNetExecutor();

    public ICommandArgument InDirectory(string workingDirectory)
    {
        _workingDirectory = workingDirectory;
        return this;
    }

    public ICommandExecutor WithArguments(string[] arguments)
    {
        _arguments = string.Join(" ", arguments);
        return this;
    }

    public ICommandResult Execute()
    {
        var dotNetProcess = new Process();

        try
        {
            dotNetProcess.SetDotNetProcessStartInfo(_workingDirectory, _arguments);
            dotNetProcess.Start();

            var getOutput = dotNetProcess.StandardOutput.ReadToEndAsync();
            var getErrors = dotNetProcess.StandardError.ReadToEndAsync();
            var dotNetProcessExited = dotNetProcess.WaitForExit(CommandConstants.TimeToExit);

            if (dotNetProcessExited)
            {
                Task.WaitAll(getOutput, getErrors);

                return new DotNetResult(getOutput.Result, getErrors.Result, dotNetProcess.ExitCode);
            }

            dotNetProcess.Kill();

            return new DotNetResult(getOutput.Result, getErrors.Result, (int) ResultStatus.Failed);
        }
        finally
        {
            dotNetProcess.Dispose();
        }
    }
}