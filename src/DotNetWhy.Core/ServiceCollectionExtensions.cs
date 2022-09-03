﻿namespace DotNetWhy.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services) =>
        services
            .AddSingleton<IDependenciesPathsProvider, DependenciesPathsProvider>()
            .AddSingleton<IDependencyGraphProvider, DependencyGraphProvider>()
            .AddSingleton<IDependencyGraphSourceProvider, DependencyGraphSourceProvider>()
            .AddSingleton<IDependencyGraphService, DependencyGraphService>()
            .AddTransient<ILockFileProvider, LockFileProvider>()
            .AddTransient<ILockFileSourceProvider, LockFileSourceProvider>();
}