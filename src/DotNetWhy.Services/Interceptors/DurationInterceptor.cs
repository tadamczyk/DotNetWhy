namespace DotNetWhy.Services.Interceptors;

internal class DurationInterceptor : IInterceptor
{
    private readonly ILogger _logger;

    public DurationInterceptor(ILogger logger)
    {
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            invocation.Proceed();
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogLine($"Time elapsed: {stopwatch.Elapsed:hh\\:mm\\:ss\\.ff}");
        }
    }
}