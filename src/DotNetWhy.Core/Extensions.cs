namespace DotNetWhy.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services) =>
        services.AddScoped<IProvider, Provider>();
}