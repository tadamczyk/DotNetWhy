namespace DotNetWhy.Domain;

public interface IDependencyTreeProvider
{
    Task<DependencyTreeNode> GetAsync(DependencyTreeParameters parameters);
}