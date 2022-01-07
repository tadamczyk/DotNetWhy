namespace DotNet.CommandExecutor;

public sealed class DotNetExecutor : ICommandDirectory,
    ICommandArgument,
    ICommandExecutor
{
    private static string _workingDirectory;
    private static string _arguments;

    private DotNetExecutor() =>
        Expression.Empty();

    public static ICommandDirectory Initialize() =>
        new DotNetExecutor();

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
        var dotNet = new Process();

        try
        {
            dotNet.SetProcessStartInfo(_workingDirectory, _arguments);
            dotNet.Start();

            var getOutput = dotNet.StandardOutput.ReadToEndAsync();
            var getErrors = dotNet.StandardError.ReadToEndAsync();
            var dotNetExited = dotNet.WaitForExit(Command.MaximumExecutionTime);

            if (dotNetExited)
            {
                Task.WaitAll(getOutput, getErrors);

                return DotNetResult.Create(getOutput.Result, getErrors.Result, dotNet.ExitCode);
            }

            dotNet.Kill();

            return DotNetResult.Create(getOutput.Result, getErrors.Result, (int) Status.Failure);
        }
        finally
        {
            dotNet.Dispose();
        }
    }
}