namespace DotNetWhy.Services.Interceptors;

internal class DurationInterceptor : IInterceptor
{
    private readonly IBaseDependencyTreeLogger _logger;
    private readonly IParameters _parameters;

    public DurationInterceptor(
        IBaseDependencyTreeLogger logger,
        IParameters parameters)
    {
        _logger = logger;
        _parameters = parameters;
    }

    public void Intercept(IInvocation invocation)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogStartMessage(_parameters.WorkingDirectory);

            invocation.Proceed();
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogEndMessage(stopwatch.Elapsed);
        }
    }
}