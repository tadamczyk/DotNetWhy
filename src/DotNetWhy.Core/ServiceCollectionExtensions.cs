namespace DotNetWhy.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services) =>
        services
            .AddSingleton<IDependencyGraphConverter, DependencyGraphConverter>()
            .AddSingleton<IDependencyGraphProvider, DependencyGraphProvider>()
            .AddSingleton<IDependencyTreeService, DependencyTreeService>()
            .AddSingleton<IDependencyGraphSourceProvider, DependencyGraphSourceProvider>()
            .AddTransient<ILockFileProvider, LockFileProvider>()
            .AddTransient<ILockFileSourceProvider, LockFileSourceProvider>();
}