namespace DotNetWhy.Application.Infrastructure;

internal static class ApplicationBuilder
{
    private static readonly IServiceCollection Services = new ServiceCollection();

    public static IServiceCollection AddServices() =>
        Services
            .AddCore()
            .AddSingleton<ILogger, Logger>();

    public static ITypeRegistrar AsTypeRegistrar(this IServiceCollection services) =>
        new TypeRegistrar(services);

    public static ICommandApp ForCommandApplication<TCommand>(
        this ITypeRegistrar typeRegistrar)
        where TCommand : class, ICommand =>
        new CommandApp<TCommand>(typeRegistrar);
}