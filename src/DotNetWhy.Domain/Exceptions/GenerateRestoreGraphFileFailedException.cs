namespace DotNetWhy.Domain.Exceptions;

internal sealed class GenerateRestoreGraphFileFailedException : Exception
{
    internal GenerateRestoreGraphFileFailedException(string workingDirectory)
        : base($"Failed to generate restore graph file of project(s) from {workingDirectory}.")
    {
    }
}