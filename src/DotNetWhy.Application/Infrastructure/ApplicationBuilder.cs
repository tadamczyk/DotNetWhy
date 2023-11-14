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
        this ITypeRegistrar typeRegistrar,
        string applicationName = null)
        where TCommand : class, ICommand
    {
        var commandApplication = new CommandApp<TCommand>(typeRegistrar);

        if (!string.IsNullOrEmpty(applicationName))
            commandApplication.Configure(configurator => configurator.SetApplicationName(applicationName));

        return commandApplication;
    }
}