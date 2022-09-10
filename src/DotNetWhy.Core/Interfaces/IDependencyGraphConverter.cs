namespace DotNetWhy.Core.Interfaces;

internal interface IDependencyGraphConverter
{
    Solution Convert(
        DependencyGraphSpec dependencyGraphSpec,
        string solutionName,
        string packageName);
}