namespace DotNet.CommandExecutor.Interfaces;

public interface ICommandDirectory
{
    ICommandArgument InDirectory(string workingDirectory);
}