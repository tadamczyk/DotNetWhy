namespace DotNet.CommandExecutor.Interfaces;

public interface ICommandArgument
{
    ICommandExecutor WithArguments(IEnumerable<string> arguments);
}