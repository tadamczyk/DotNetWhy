namespace DotNet.Cli;

/// <summary>
///     Represents a factory for creating "dotnet" command instances.
/// </summary>
public interface ICommandFactory
{
    /// <summary>
    ///     Creates a new instance of "dotnet" <see cref="Command" /> class.
    /// </summary>
    /// <returns>A new "dotnet" <see cref="Command" /> instance.</returns>
    Command Create();
}