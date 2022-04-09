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
        _workingDirectory = workingDirectory ?? Environment.CurrentDirectory;
        return this;
    }

    public ICommandExecutor WithArguments(IEnumerable<string> arguments)
    {
        _arguments = string.Join(" ", arguments ?? Array.Empty<string>());
        return this;
    }

    public DotNetResult Execute()
    {
        var dotNet = new Process();

        try
        {
            dotNet.SetProcessStartInfo(_workingDirectory, _arguments);
            dotNet.Start();

            var getOutputAsync = dotNet.StandardOutput.ReadToEndAsync();
            var getErrorsAsync = dotNet.StandardError.ReadToEndAsync();
            var dotNetCommandDone = dotNet.WaitForExit(TimeConstants.MaximumExecutionTime);

            if (dotNetCommandDone)
            {
                Task.WaitAll(getOutputAsync, getErrorsAsync);

                return DotNetResult.Create(getOutputAsync.Result, getErrorsAsync.Result, dotNet.ExitCode);
            }

            dotNet.Kill();

            return DotNetResult.Create(getOutputAsync.Result, getErrorsAsync.Result, (int) Status.Failure);
        }
        finally
        {
            dotNet.Dispose();
        }
    }
}