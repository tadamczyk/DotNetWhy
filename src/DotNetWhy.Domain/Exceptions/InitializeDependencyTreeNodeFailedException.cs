namespace DotNetWhy.Domain.Exceptions;

internal sealed class InitializeDependencyTreeNodeFailedException : Exception
{
    internal InitializeDependencyTreeNodeFailedException()
        : base("Cannot create new DependencyTreeNode object using parameterless constructor. Please use new DependencyTreeNode(string name, string version = null) constructor.")
    {
    }
}