namespace DotNet.CommandExecutor;

public sealed class DotNetExecutor
{
    private readonly string _arguments;
    internal const string ArgumentsFieldName = nameof(_arguments);

    private readonly string _workingDirectory;
    internal const string WorkingDirectoryFieldName = nameof(_workingDirectory);

    private DotNetExecutor()
    {
        _arguments = string.Empty;
        _workingDirectory = Environment.CurrentDirectory;
    }

    public static DotNetExecutor Initialize() => new();

    public DotNetResult AndExecute()
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