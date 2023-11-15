namespace DotNetWhy.Core.Commands;

internal sealed class DotNetCliCommand
(
    string workingDirectory,
    IEnumerable<string> arguments
) : IResultHandler
{
    private const int SuccessStatusCode = 0;
    private const int TimeoutMilliseconds = 30000;
    private const string ArgumentsSeparator = " ";
    private const string ProcessName = "dotnet";

    public Result Handle()
    {
        var process = new Process();

        try
        {
            process.StartInfo = new ProcessStartInfo(ProcessName, string.Join(ArgumentsSeparator, arguments))
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };

            process.Start();

            var getError = process.StandardError.ReadToEndAsync();
            var getOutput = process.StandardOutput.ReadToEndAsync();

            var processExited = process.WaitForExit(TimeoutMilliseconds);

            if (processExited)
            {
                var isSuccess = process.ExitCode is SuccessStatusCode;

                if (isSuccess) return Result.Success();

                var error = getError.Result;

                return Result.Failure(error);
            }

            process.Kill();

            return Result.Failure(Errors.ProcessNotExited);
        }
        catch (Exception exception)
        {
            return Result.Failure(exception.Message);
        }
        finally
        {
            process.Dispose();
        }
    }

    private static class Errors
    {
        public const string ProcessNotExited = $"'{ProcessName}' process has not exited";
    }
}
