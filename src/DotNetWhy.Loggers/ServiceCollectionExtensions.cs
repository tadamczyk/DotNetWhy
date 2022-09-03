namespace DotNetWhy.Loggers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggers(this IServiceCollection services) =>
        services
            .AddSingleton<ILogger, ConsoleLogger>();
}