namespace DotNetWhy.Domain.Exceptions;

internal sealed class RestoreProjectFailedException : Exception
{
    internal RestoreProjectFailedException(string workingDirectory)
        : base($"Failed to restore project(s) from {workingDirectory}.")
    {
    }
}