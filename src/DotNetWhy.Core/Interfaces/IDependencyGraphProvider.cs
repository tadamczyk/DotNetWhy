namespace DotNetWhy.Core.Interfaces;

internal interface IDependencyGraphProvider
{
    DependencyGraphSpec Get(string workingDirectory);
}