namespace DotNet.CommandExecutor;

/// <summary>
///     The executor of the DotNet process.
/// </summary>
public sealed class DotNetExecutor
{
    internal string Arguments { get; set; }
    internal string WorkingDirectory { get; set; }

    private DotNetExecutor()
    {
        Arguments = string.Empty;
        WorkingDirectory = Environment.CurrentDirectory;
    }

    /// <summary>
    ///     Creates a new instance of the DotNet process for future execution.
    /// </summary>
    /// <returns>A new instance of <see cref="DotNetExecutor" />.</returns>
    public static DotNetExecutor Initialize() => new();

    /// <summary>
    ///     Executes prepared instance of the DotNet process.
    /// </summary>
    /// <returns>A result of performed the DotNet process as a <see cref="DotNetResult" />.</returns>
    public DotNetResult AndExecute()
    {
        var dotNet = new Process();

        try
        {
            dotNet.SetProcessStartInfo(WorkingDirectory, Arguments);
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