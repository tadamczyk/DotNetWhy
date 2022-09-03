namespace DotNetWhy.Services.Interceptors;

internal class DurationInterceptor : IInterceptor
{
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

            Console.WriteLine($"Time elapsed: {stopwatch.Elapsed:hh\\:mm\\:ss\\.ff}");
        }
    }
}