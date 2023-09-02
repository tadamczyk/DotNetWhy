namespace DotNetWhy.Loggers;

public interface IDependencyTreeLogger : IBaseDependencyTreeLogger
{
    void LogResults(
        DependencyTreeNode solution,
        string packageName,
        string packageVersion);
}