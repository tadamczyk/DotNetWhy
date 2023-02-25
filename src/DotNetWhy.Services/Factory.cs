namespace DotNetWhy.Services;

public static class Factory
{
    public static IDotNetWhyService Create() =>
        new ServiceCollection()
            .AddServices()
            .BuildServiceProvider()
            .GetRequiredService<IDotNetWhyService>();

    private static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddInterceptedSingleton<IDotNetWhyService, DotNetWhyService, DurationInterceptor>()
            .AddCore()
            .AddLoggers()
            .AddValidators();

    private static IServiceCollection AddInterceptedSingleton<TInterface, TImplementation, TInterceptor>(
        this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        where TInterceptor : class, IInterceptor
    {
        services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();
        services.TryAddTransient<TInterceptor>();
        services.AddSingleton<TImplementation>();

        services.AddSingleton(provider =>
        {
            var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();
            var implementation = provider.GetRequiredService<TImplementation>();
            var interceptor = provider.GetRequiredService<TInterceptor>();

            return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(
                implementation,
                interceptor);
        });

        return services;
    }
}