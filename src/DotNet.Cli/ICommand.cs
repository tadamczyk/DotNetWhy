namespace DotNet.Cli;

/// <summary>
///     Represents "dotnet" command that can be executed in the command-line interface.
/// </summary>
public interface ICommand
{
    /// <summary>
    ///     Gets the arguments to be passed when executing "dotnet" command.
    /// </summary>
    string Arguments { get; }

    /// <summary>
    ///     Gets the working directory for "dotnet" command execution.
    /// </summary>
    string WorkingDirectory { get; }

    /// <summary>
    ///     Sets the arguments for "dotnet" command.
    /// </summary>
    /// <param name="arguments">The arguments to be set.</param>
    /// <returns>The current "dotnet" <see cref="ICommand" /> instance.</returns>
    ICommand WithArguments(IEnumerable<string> arguments);

    /// <summary>
    ///     Sets the arguments for "dotnet" command.
    /// </summary>
    /// <param name="arguments">The arguments to be set.</param>
    /// <returns>The current "dotnet" <see cref="ICommand" /> instance.</returns>
    ICommand WithArguments(string arguments);

    /// <summary>
    ///     Sets the working directory for "dotnet" command.
    /// </summary>
    /// <param name="workingDirectory">The working directory to be set.</param>
    /// <returns>The current "dotnet" <see cref="ICommand" /> instance.</returns>
    ICommand WithWorkingDirectory(string workingDirectory);

    /// <summary>
    ///     Executes "dotnet" command and returns <see cref="ICommandResult" />.
    /// </summary>
    /// <returns>An instance of <see cref="ICommandResult" /> being the result of the command execution.</returns>
    ICommandResult Execute();
}