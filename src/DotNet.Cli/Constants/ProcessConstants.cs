namespace DotNet.Cli.Constants;

internal static class ProcessConstants
{
    public const string Name = "dotnet";
    public const int SuccessStatusCode = 0;

    public static readonly int Timeout = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;
}