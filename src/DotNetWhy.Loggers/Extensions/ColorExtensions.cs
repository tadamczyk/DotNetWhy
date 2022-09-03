namespace DotNetWhy.Loggers.Extensions;

internal static class ColorExtensions
{
    public static ConsoleColor Parse(this Color color) =>
        Enum.Parse<ConsoleColor>(color.ToString());
}