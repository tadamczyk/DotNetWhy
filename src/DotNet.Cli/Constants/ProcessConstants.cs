namespace DotNet.Cli.Constants;

internal static class ProcessConstants
{
    public const string Name = "dotnet";
    public const string NotExitedError = $"'{Name}' process has not exited";

    public const int SuccessStatusCode = 0;
    public const int TimeoutMilliseconds = 30000;
}