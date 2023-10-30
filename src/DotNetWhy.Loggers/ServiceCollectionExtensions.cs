namespace DotNetWhy.Loggers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLoggers(this IServiceCollection services) =>
        services
            .AddCore()
            .AddSingleton<IBaseDependencyTreeLogger, BaseDependencyTreeLogger>()
            .AddSingleton<IDependencyTreeLogger, ConsoleDependencyTreeLogger>()
            .AddSingleton<ILogger, ConsoleLogger>();
}