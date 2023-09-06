namespace DotNet.Cli.Factories;

internal sealed class CommandFactory : ICommandFactory
{
    public Command Create() => new();
}