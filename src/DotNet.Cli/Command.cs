namespace DotNet.Cli;

public sealed class Command
{
    public string Arguments { get; private set; }
    public string WorkingDirectory { get; private set; }

    private Command()
    {
        Arguments = CommandDefaults.Arguments;
        WorkingDirectory = CommandDefaults.WorkingDirectory;
    }

    public static Command Create() =>
        new();

    public Command WithArguments(IEnumerable<string> arguments) =>
        WithArguments(string.Join(CommandConstants.ArgumentsSeparator, arguments));

    public Command WithArguments(string arguments)
    {
        Arguments = arguments;

        return this;
    }

    public Command WithWorkingDirectory(string workingDirectory)
    {
        WorkingDirectory = workingDirectory;

        return this;
    }

    public CommandResult Execute()
    {
        var process = new Process();

        try
        {
            process.StartInfo = new ProcessStartInfo(CommandConstants.ProcessName, Arguments)
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

            var processExited = process.WaitForExit(CommandConstants.Timeout);

            if (processExited)
            {
                Task.WaitAll(readOutput, readError);

                var isSuccess = process.ExitCode == CommandConstants.SuccessStatusCode;

                return isSuccess
                    ? CommandResult.Success(readOutput.Result)
                    : CommandResult.Fail(readOutput.Result, readError.Result);
            }

            process.Kill();

            return CommandResult.Fail();
        }
        catch (Exception exception)
        {
            return CommandResult.Fail(error: exception.Message);
        }
        finally
        {
            process.Dispose();
        }
    }
}