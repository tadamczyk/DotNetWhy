namespace DotNetWhy.Loggers;

public interface IDependencyTreeLogger : IDependencyTreeResultsLogger
{
    void LogStartMessage(string workingDirectory);
    void LogErrors(IEnumerable<string> errors);
    void LogEndMessage(TimeSpan elapsedTime);
}