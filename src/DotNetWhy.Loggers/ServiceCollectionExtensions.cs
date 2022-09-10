namespace DotNetWhy.Loggers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggers(this IServiceCollection services) =>
        services
            .AddSingleton<IDependencyTreeLogger, DependencyTreeLogger>()
            .AddSingleton<ILogger, ConsoleLogger>();
}