namespace DotNetWhy.Loggers.Services;

internal class ConsoleLogger : ILogger
{
    public ConsoleLogger()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    public void Log(string text = "", ConsoleColor? color = null)
    {
        if (color.HasValue) Console.ForegroundColor = color.Value;
        Console.Write(text);
        Console.ResetColor();
    }

    public void LogLine(string text = "", ConsoleColor? color = null)
    {
        if (color.HasValue) Console.ForegroundColor = color.Value;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}