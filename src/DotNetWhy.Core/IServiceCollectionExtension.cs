namespace DotNetWhy.Core;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services) =>
        services
            .AddSingleton<IDependencyGraphProvider, DependencyGraphProvider>()
            .AddSingleton<IDependencyGraphSourceProvider, DependencyGraphSourceProvider>()
            .AddSingleton<IDependencyGraphService, DependencyGraphService>()
            .AddSingleton<ILockFileProvider, LockFileProvider>()
            .AddSingleton<ILockFileSourceProvider, LockFileSourceProvider>();
}