namespace DotNet.Processor;

public interface IDotNetCommandInDirectory
{
    IDotNetCommandWithArguments InDirectory(string workingDirectory);
}