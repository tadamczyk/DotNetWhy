namespace DotNetWhy.Core.Interfaces;

internal interface IDependencyGraphConverter
{
    Solution ToSolution(DependencyGraphSpec dependencyGraphSpec, string solutionName, string packageName);
}