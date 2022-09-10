namespace DotNetWhy.Loggers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggers(this IServiceCollection services) =>
        services
            .AddSingleton<IBaseDependencyTreeLogger, BaseDependencyTreeLogger>()
            .AddSingleton<IDependencyTreeLogger, ConsoleDependencyTreeLogger>()
            .AddSingleton<ILogger, ConsoleLogger>();
}