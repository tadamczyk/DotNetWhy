namespace DotNetWhy.Application.Loggers;

internal static class Extensions
{
    public static FormattableString Bold(this string value) => $"[bold]{value}[/]";
    public static FormattableString DarkCyan(this string value) => $"[teal]{value}[/]";
    public static FormattableString DarkGreen(this string value) => $"[green]{value}[/]";
    public static FormattableString Green(this string value) => $"[lime]{value}[/]";
    public static FormattableString Red(this string value) => $"[red]{value}[/]";
}