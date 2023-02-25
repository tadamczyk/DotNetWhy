namespace DotNet.CommandExecutor;

/// <summary>
///     The extension methods for setting internal properties of <see cref="DotNetExecutor" />.
/// </summary>
public static class DotNetExecutorExtensions
{
    /// <summary>
    ///     Set a working directory in which the <see cref="DotNetExecutor" /> has to be execute.
    ///     If passed parameter is null then set current directory as working directory.
    /// </summary>
    /// <param name="dotNetExecutor">The extensible instance of <see cref="DotNetExecutor" />.</param>
    /// <param name="workingDirectory">The working directory in which the <see cref="DotNetExecutor" /> has to be execute.</param>
    /// <returns>The <see cref="DotNetExecutor" />.</returns>
    public static DotNetExecutor InDirectory(
        this DotNetExecutor dotNetExecutor,
        string workingDirectory)
    {
        dotNetExecutor.WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory;
        return dotNetExecutor;
    }

    /// <summary>
    ///     Set the arguments with which the <see cref="DotNetExecutor" /> has to be execute.
    ///     If passed parameters are null then set no argument.
    /// </summary>
    /// <param name="dotNetExecutor">The extensible instance of <see cref="DotNetExecutor" />.</param>
    /// <param name="arguments">The arguments with which the <see cref="DotNetExecutor" /> has to be execute.</param>
    /// <returns>The <see cref="DotNetExecutor" />.</returns>
    public static DotNetExecutor WithArguments(
        this DotNetExecutor dotNetExecutor,
        IEnumerable<string> arguments)
    {
        dotNetExecutor.Arguments = string.Join(" ", arguments ?? Array.Empty<string>());
        return dotNetExecutor;
    }
}