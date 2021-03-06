namespace DotNetWhy.Core.Exceptions;

internal class GenerateGraphFileFailedException : Exception
{
    internal GenerateGraphFileFailedException(string workingDirectory)
        : base($"Failed to generate dependencies graph file of project(s) from {workingDirectory}.") =>
        Expression.Empty();
}