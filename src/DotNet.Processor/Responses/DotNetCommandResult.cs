namespace DotNet.Processor;

public class DotNetCommandResult
{
    private string Output { get; }
    private string Errors { get; }
    private int Status { get; }

    public DotNetCommandResult(string output, string errors, int status) =>
        (Output, Errors, Status) = (output, errors, status);
}