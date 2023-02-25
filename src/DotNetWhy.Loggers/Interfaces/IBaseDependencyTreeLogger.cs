namespace DotNetWhy.Loggers;

public interface IBaseDependencyTreeLogger
{
    void LogStartMessage(string workingDirectory);
    void LogErrors(IEnumerable<string> errors);
    void LogEndMessage(TimeSpan elapsedTime);
}