namespace DotNetWhy.Core.Exceptions;

internal class RestoreProjectFailedException : Exception
{
    public RestoreProjectFailedException(string workingDirectory)
        : base($"Failed to restore project(s) from {workingDirectory}.") =>
        Expression.Empty();
}