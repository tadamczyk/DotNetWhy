namespace DotNet.Processor;

public interface IDotNetCommandWithArguments
{
    IDotNetCommandExecute WithArguments(string[] arguments);
}