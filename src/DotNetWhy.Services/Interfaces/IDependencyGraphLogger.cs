namespace DotNetWhy.Services.Interfaces;

internal interface IDependencyGraphLogger
{
    void Log(SolutionDependencyGraph solutionDependencyGraph, string packageName);
}