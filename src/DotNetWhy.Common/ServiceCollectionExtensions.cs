namespace DotNetWhy.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services) =>
        services.AddScoped<IMediator, Mediator.Mediator>();
}