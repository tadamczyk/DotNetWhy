namespace DotNetWhy.Loggers.Interfaces;

internal interface ILogger
{
    void Log(
        char character,
        int repeat,
        Color? color = null);

    void Log(
        string text,
        Color? color = null);

    void LogLine(
        string text = "",
        Color? color = null);
}