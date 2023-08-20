﻿namespace DotNet.Cli;

/// <summary>
///     Represents the result of "dotnet" command execution.
/// </summary>
public sealed class CommandResult
{
    private CommandResult(
        bool isSuccess,
        string output,
        string error)
    {
        IsSuccess = isSuccess;
        Output = output;
        Error = error;
    }

    /// <summary>
    ///     Gets a value indicating whether "dotnet" command execution was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    ///     Gets the standard output generated by "dotnet" command execution.
    /// </summary>
    public string Output { get; }

    /// <summary>
    ///     Gets the error output generated by "dotnet" command execution.
    /// </summary>
    public string Error { get; }

    /// <summary>
    ///     Creates a new instance of <see cref="CommandResult" /> representing a successful "dotnet" command execution.
    /// </summary>
    /// <param name="output">The standard output generated by "dotnet" command.</param>
    /// <returns>An instance of <see cref="CommandResult" /> indicating success.</returns>
    internal static CommandResult Success(string output) =>
        new(
            true,
            output,
            CommandResultDefaults.Output);

    /// <summary>
    ///     Creates a new instance of <see cref="CommandResult" /> representing a failed "dotnet" command execution.
    /// </summary>
    /// <param name="error">The error output generated by "dotnet" command.</param>
    /// <param name="output">The standard output generated by "dotnet" command.</param>
    /// <returns>An instance of <see cref="CommandResult" /> indicating failure.</returns>
    internal static CommandResult Failure(string error, string output = null) =>
        new(
            false,
            output ?? CommandResultDefaults.Output,
            error);
}