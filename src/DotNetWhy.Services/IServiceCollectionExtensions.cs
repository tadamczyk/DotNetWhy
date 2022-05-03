namespace DotNetWhy.Services;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddSingleton<IDependencyGraphLogger, DependencyGraphLogger>()
            .AddSingleton<IDotNetWhyService, DotNetWhyService>()
            .AddSingleton<IFileSystem, FileSystem>()
            .AddCore()
            .AddValidators();
}