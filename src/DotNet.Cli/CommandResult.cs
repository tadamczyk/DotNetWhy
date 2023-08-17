namespace DotNet.Cli;

public sealed class CommandResult
{
    public bool IsSuccess { get; }
    public string Output { get; }
    public string Error { get; }

    private CommandResult(
        bool isSuccess,
        string output,
        string error)
    {
        IsSuccess = isSuccess;
        Output = output;
        Error = error;
    }

    internal static CommandResult Success(string output = "") =>
        new(
            true,
            output,
            string.Empty);

    internal static CommandResult Fail(string output = "", string error = "") =>
        new(
            false,
            output,
            error);
}