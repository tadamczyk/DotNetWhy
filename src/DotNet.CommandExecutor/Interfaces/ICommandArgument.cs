namespace DotNet.CommandExecutor.Interfaces;

public interface ICommandArgument
{
    ICommandExecutor WithArguments(string[] arguments);
}