namespace DotNetWhy.Domain;

public interface IDependencyTreeProvider
{
    Task<DependencyTreeNode> GetAsync(
        string workingDirectory,
        string packageName,
        string packageVersion = null);
}