namespace DotNetWhy.Loggers.Extensions;

internal static class ConsoleLoggerExtensions
{
    public static void Set(this Color color) =>
        Console.ForegroundColor = Enum.Parse<ConsoleColor>(color.ToString());

    public static void Set(this Encoding encoding) =>
        Console.OutputEncoding = encoding;
}