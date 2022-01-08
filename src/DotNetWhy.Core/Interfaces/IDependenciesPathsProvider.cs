namespace DotNetWhy.Core.Interfaces;

internal interface IDependenciesPathsProvider
{
    IReadOnlyCollection<DependenciesPath[]> GetByPackageName(string dependenciesPath, string packageName);
}