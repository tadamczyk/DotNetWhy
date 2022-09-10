namespace DotNetWhy.Loggers;

public interface IDependencyTreeLogger : IBaseDependencyTreeLogger
{
    void LogResults(
        Solution solution,
        string packageName);
}