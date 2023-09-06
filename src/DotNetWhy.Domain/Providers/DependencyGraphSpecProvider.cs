namespace DotNetWhy.Domain.Providers;

internal interface IDependencyGraphSpecProvider
{
    DependencyGraphSpec Get(string path);
}

internal sealed class DependencyGraphSpecProvider : IDependencyGraphSpecProvider
{
    public DependencyGraphSpec Get(string path) =>
        DependencyGraphSpec.Load(path);
}