namespace DotNetWhy.Loggers.Services;

internal class ConsoleLogger : ILogger
{
    public LoggerConfiguration Configuration => new(Console.WindowWidth);

    public ConsoleLogger()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    public void Log(string text = "", Color? color = null)
    {
        if (color.HasValue) Console.ForegroundColor = color.Value.Parse();
        Console.Write(text);
        Console.ResetColor();
    }

    public void LogLine(string text = "", Color? color = null)
    {
        if (color.HasValue) Console.ForegroundColor = color.Value.Parse();
        Console.WriteLine(text);
        Console.ResetColor();
    }
}