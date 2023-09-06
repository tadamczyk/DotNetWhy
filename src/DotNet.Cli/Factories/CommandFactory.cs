namespace DotNet.Cli.Factories;

internal sealed class CommandFactory : ICommandFactory
{
    public ICommand Create() =>
        new Command();
}