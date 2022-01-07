namespace DotNet.CommandExecutor;

public sealed class DotNetResult : ICommandResult
{
    public string Output { get; }
    public string Errors { get; }
    public bool IsSuccess => Status is (int) Constants.Status.Success;

    private int Status { get; }

    private DotNetResult(string output, string errors, int status) =>
        (Output, Errors, Status) = (output, errors, status);

    internal static DotNetResult Create(string output, string errors, int status) =>
        new(output, errors, status);
}