namespace DotNet.CommandExecutor.Interfaces;

public interface ICommandResult
{
    public string Output { get; }
    public string Errors { get; }
    public int Status { get; }
    public bool IsSuccess { get; }
}