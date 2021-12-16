namespace DotNet.CommandExecutor;

public sealed class DotNetResult : ICommandResult
{
    public string Output { get; }
    public string Errors { get; }
    public int Status { get; }
    public bool IsSuccess => Status is (int) ResultStatus.Success;

    public DotNetResult(string output, string errors, int status) =>
        (Output, Errors, Status) = (output, errors, status);
}