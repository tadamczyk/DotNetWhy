namespace DotNet.Cli.Factories;

/// <summary>
///     Represents a factory for creating "dotnet" command instances.
/// </summary>
internal sealed class CommandFactory : ICommandFactory
{
    /// <summary>
    ///     Creates a new instance of "dotnet" <see cref="ICommand" /> class.
    /// </summary>
    /// <returns>A new "dotnet" <see cref="ICommand" /> instance.</returns>
    public ICommand Create() =>
        new Command();
}