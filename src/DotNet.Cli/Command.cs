namespace DotNet.Cli;

/// <summary>
///     Represents "dotnet" command that can be executed in the command-line interface.
/// </summary>
public sealed class Command
{
    private Command()
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
    ///     Creates a new instance of "dotnet" <see cref="Command" /> class.
    /// </summary>
    /// <returns>A new "dotnet" <see cref="Command" /> instance.</returns>
    public static Command Create() =>
        new();

    /// <summary>
    ///     Sets the arguments for "dotnet" command.
    /// </summary>
    /// <param name="arguments">The arguments to be set.</param>
    /// <returns>The current "dotnet" <see cref="Command" /> instance.</returns>
    public Command WithArguments(IEnumerable<string> arguments) =>
        WithArguments(string.Join(CommandConstants.ArgumentsSeparator, arguments));

    /// <summary>
    ///     Sets the arguments for "dotnet" command.
    /// </summary>
    /// <param name="arguments">The arguments to be set.</param>
    /// <returns>The current "dotnet" <see cref="Command" /> instance.</returns>
    public Command WithArguments(string arguments)
    {
        Arguments = arguments;

        return this;
    }

    /// <summary>
    ///     Sets the working directory for "dotnet" command.
    /// </summary>
    /// <param name="workingDirectory">The working directory to be set.</param>
    /// <returns>The current "dotnet" <see cref="Command" /> instance.</returns>
    public Command WithWorkingDirectory(string workingDirectory)
    {
        WorkingDirectory = workingDirectory;

        return this;
    }

    /// <summary>
    ///     Executes "dotnet" command and returns <see cref="CommandResult" />.
    /// </summary>
    /// <returns>An instance of <see cref="CommandResult" /> being the result of the command execution.</returns>
    public CommandResult Execute()
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

            var processExited = process.WaitForExit(ProcessConstants.Timeout);

            if (processExited)
            {
                Task.WaitAll(readOutput, readError);

                var isSuccess = process.ExitCode is ProcessConstants.SuccessStatusCode;

                return isSuccess
                    ? CommandResult.Success(readOutput.Result)
                    : CommandResult.Failure(readError.Result, readOutput.Result);
            }

            process.Kill();

            return CommandResult.Failure(CommandResultConstants.ProcessHasNotExitedError);
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