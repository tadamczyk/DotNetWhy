namespace DotNet.Cli;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCli(this IServiceCollection services) =>
        services.AddSingleton<ICommandFactory, CommandFactory>();
}