namespace DotNetWhy;

internal static class IServiceProviderExtension
{
    internal static IServiceProvider GetServiceProvider() =>
        new ServiceCollection()
            .AddSingleton<IDependencyGraphLogger, DependencyGraphLogger>()
            .AddSingleton<IFileSystem, FileSystem>()
            .AddCoreServices()
            .BuildServiceProvider();
}