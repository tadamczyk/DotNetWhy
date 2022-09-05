namespace DotNetWhy.Core;

public interface IDependencyGraphService
{
    Solution GetConvertedDependencyGraphByPackageName(string workingDirectory, string packageName);
}