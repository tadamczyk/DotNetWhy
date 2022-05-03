namespace DotNetWhy.Services.Wrappers;

internal static class StopwatchLogger
{
    public static void Log(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();

        Console.WriteLine(GetElapsedTimeSpanMessage(stopwatch.Elapsed));
    }

    private static string GetElapsedTimeSpanMessage(TimeSpan elapsedTimeSpan) =>
        $"Time elapsed: {elapsedTimeSpan:hh\\:mm\\:ss\\.ff}";
}