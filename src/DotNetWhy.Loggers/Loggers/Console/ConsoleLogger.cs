namespace DotNetWhy.Loggers.Console;

internal class ConsoleLogger : ILogger
{
    public ConsoleLogger()
    {
        ConsoleLoggerConstants.Encoding.Set();
    }

    public void Log(
        char character,
        int repeat,
        Color? color = null) =>
        Log(new string(character, repeat), color);

    public void Log(
        string text,
        Color? color = null) =>
        Wrapper(() => System.Console.Write(text), color);

    public void LogLine(
        string text = "",
        Color? color = null) =>
        Log(text + Environment.NewLine, color);

    private static void Wrapper(
        Action log,
        Color? color = null)
    {
        color?.Set();
        log();
        System.Console.ResetColor();
    }
}