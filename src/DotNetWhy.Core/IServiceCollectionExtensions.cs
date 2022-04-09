namespace DotNetWhy.Core;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services) =>
        services
            .AddSingleton<IDependenciesPathsProvider, DependenciesPathsProvider>()
            .AddSingleton<IDependencyGraphProvider, DependencyGraphProvider>()
            .AddSingleton<IDependencyGraphSourceProvider, DependencyGraphSourceProvider>()
            .AddSingleton<IDependencyGraphService, DependencyGraphService>()
            .AddTransient<ILockFileProvider, LockFileProvider>()
            .AddTransient<ILockFileSourceProvider, LockFileSourceProvider>();
}