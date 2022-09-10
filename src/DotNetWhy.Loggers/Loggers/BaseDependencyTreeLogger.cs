namespace DotNetWhy.Loggers;

internal class BaseDependencyTreeLogger : IBaseDependencyTreeLogger
{
    private readonly ILogger _logger;

    public BaseDependencyTreeLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void LogStartMessage(string workingDirectory)
    {
        _logger.LogLine($"Analyzing project(s) from {workingDirectory} directory...");
        _logger.LogLine();
    }

    public void LogErrors(IEnumerable<string> errors)
    {
        errors.ForEach(error => _logger.LogLine(error, Color.Red));
    }

    public void LogEndMessage(TimeSpan elapsedTime)
    {
        _logger.LogLine($"Time elapsed: {elapsedTime:hh\\:mm\\:ss\\.ff}");
    }
}