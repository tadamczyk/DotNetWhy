namespace DotNet.Cli.Constants;

internal static class CommandConstants
{
    public static readonly int Timeout = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;

    public const int SuccessStatusCode = 0;

    public const string ArgumentsSeparator = " ";
    public const string ProcessName = "dotnet";
}