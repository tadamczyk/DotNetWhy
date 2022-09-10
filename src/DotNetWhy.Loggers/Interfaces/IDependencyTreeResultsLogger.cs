namespace DotNetWhy.Loggers;

public interface IDependencyTreeResultsLogger
{
    void LogResults(
        Solution solution,
        string packageName);
}