namespace DotNetWhy;

internal static class IServiceCollectionExtensions
{
    private static readonly IServiceCollection Services;

    static IServiceCollectionExtensions() =>
        Services = new ServiceCollection();

    internal static IDotNetWhyService GetDotNetWhyService() =>
        GetServiceProvider()
            .GetService<IDotNetWhyService>();

    private static IServiceProvider GetServiceProvider() =>
        Services
            .AddServices()
            .BuildServiceProvider();
}