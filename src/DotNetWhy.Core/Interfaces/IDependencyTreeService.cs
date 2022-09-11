namespace DotNetWhy.Core;

public interface IDependencyTreeService
{
    Solution GetDependencyTreeByPackageName(
        string workingDirectory,
        string packageName);
}