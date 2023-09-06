namespace DotNet.Cli;

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
    /// <returns>The current "dotnet" <see cref="Command" /> instance.</returns>
    ICommand WithArguments(IEnumerable<string> arguments);

    /// <summary>
    ///     Sets the arguments for "dotnet" command.
    /// </summary>
    /// <param name="arguments">The arguments to be set.</param>
    /// <returns>The current "dotnet" <see cref="Command" /> instance.</returns>
    ICommand WithArguments(string arguments);

    /// <summary>
    ///     Sets the working directory for "dotnet" command.
    /// </summary>
    /// <param name="workingDirectory">The working directory to be set.</param>
    /// <returns>The current "dotnet" <see cref="Command" /> instance.</returns>
    ICommand WithWorkingDirectory(string workingDirectory);

    /// <summary>
    ///     Executes "dotnet" command and returns <see cref="CommandResult" />.
    /// </summary>
    /// <returns>An instance of <see cref="CommandResult" /> being the result of the command execution.</returns>
    CommandResult Execute();
}