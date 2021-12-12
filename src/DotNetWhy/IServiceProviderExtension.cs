namespace DotNetWhy;

internal static class IServiceProviderExtension
{
    internal static IServiceProvider GetServiceProvider() =>
        new ServiceCollection()
            .AddSingleton<IFileSystem, FileSystem>()
            .AddCoreServices()
            .BuildServiceProvider();
}