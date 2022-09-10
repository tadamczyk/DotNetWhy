namespace DotNetWhy.Services.Interceptors;

internal class DurationInterceptor : IInterceptor
{
    private readonly IDependencyTreeLogger _logger;

    public DurationInterceptor(IDependencyTreeLogger logger)
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

            _logger.LogEndMessage(stopwatch.Elapsed);
        }
    }
}