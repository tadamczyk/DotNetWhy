namespace DotNetWhy.Loggers;

public interface ILogger
{
    void Log(string text = "", ConsoleColor? color = null);
    void LogLine(string text = "", ConsoleColor? color = null);
}