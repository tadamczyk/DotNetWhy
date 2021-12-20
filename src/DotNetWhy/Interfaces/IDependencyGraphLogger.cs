namespace DotNetWhy.Interfaces;

internal interface IDependencyGraphLogger
{
    void Log(SolutionDependencyGraph solutionDependencyGraph, string packageName);
}