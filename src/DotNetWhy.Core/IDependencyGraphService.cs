namespace DotNetWhy.Core;

public interface IDependencyGraphService
{
    SolutionDependencyGraph GetDependencyGraphByPackageName(string workingDirectory, string packageName);
}