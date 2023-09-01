namespace DotNetWhy.Loggers;

public interface IDependencyTreeLogger : IBaseDependencyTreeLogger
{
    void LogResults(
        Node solution,
        string packageName,
        string packageVersion);
}