namespace DotNet.Cli.Commands;

/// <summary>
///     Represents "dotnet" command that can be executed in the command-line interface.
/// </summary>
internal sealed class Command : ICommand
{
    internal Command()
    {
        Arguments = CommandDefaults.Arguments;
        WorkingDirectory = CommandDefaults.WorkingDirectory;
    }

    /// <summary>
    ///     Gets the arguments to be passed when executing "dotnet" command.
    /// </summary>
    public string Arguments { get; private set; }

    /// <summary>
    ///     Gets the working directory for "dotnet" command execution.
    /// </summary>
    public string WorkingDirectory { get; private set; }

    /// <summary>
    ///     Sets the arguments for "dotnet" command.
    /// </summary>
    /// <param name="arguments">The arguments to be set.</param>
    /// <returns>The current "dotnet" <see cref="ICommand" /> instance.</returns>
    public ICommand WithArguments(IEnumerable<string> arguments) =>
        WithArguments(string.Join(CommandConstants.ArgumentsSeparator, arguments));

    /// <summary>
    ///     Sets the arguments for "dotnet" command.
    /// </summary>
    /// <param name="arguments">The arguments to be set.</param>
    /// <returns>The current "dotnet" <see cref="ICommand" /> instance.</returns>
    public ICommand WithArguments(string arguments)
    {
        Arguments = arguments;

        return this;
    }

    /// <summary>
    ///     Sets the working directory for "dotnet" command.
    /// </summary>
    /// <param name="workingDirectory">The working directory to be set.</param>
    /// <returns>The current "dotnet" <see cref="ICommand" /> instance.</returns>
    public ICommand WithWorkingDirectory(string workingDirectory)
    {
        WorkingDirectory = workingDirectory;

        return this;
    }

    /// <summary>
    ///     Executes "dotnet" command and returns <see cref="ICommandResult" />.
    /// </summary>
    /// <returns>An instance of <see cref="ICommandResult" /> being the result of the command execution.</returns>
    public ICommandResult Execute()
    {
        var process = new Process();

        try
        {
            process.StartInfo = new ProcessStartInfo(ProcessConstants.Name, Arguments)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory
            };

            process.Start();

            var readOutput = process.StandardOutput.ReadToEndAsync();
            var readError = process.StandardError.ReadToEndAsync();

            var processExited = process.WaitForExit(ProcessConstants.TimeoutMilliseconds);

            if (processExited)
            {
                Task.WaitAll(readOutput, readError);

                var isSuccess = process.ExitCode is ProcessConstants.SuccessStatusCode;

                return isSuccess
                    ? CommandResult.Success(readOutput.Result)
                    : CommandResult.Failure(readError.Result, readOutput.Result);
            }

            process.Kill();

            return CommandResult.Failure(ProcessConstants.NotExitedError);
        }
        catch (Exception exception)
        {
            return CommandResult.Failure(exception.Message);
        }
        finally
        {
            process.Dispose();
        }
    }
}