namespace DotNetWhy.Services.Interceptors;

internal class DurationInterceptor : IInterceptor
{
    private readonly IBaseDependencyTreeLogger _logger;

    public DurationInterceptor(IBaseDependencyTreeLogger logger) => _logger = logger;

    public void Intercept(IInvocation invocation)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogStartMessage(Environment.CurrentDirectory);

            invocation.Proceed();
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogEndMessage(stopwatch.Elapsed);
        }
    }
}