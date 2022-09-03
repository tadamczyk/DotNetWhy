namespace DotNetWhy.Validators;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services) =>
        services
            .AddScoped<IValidatorsWrapper, ValidatorsWrapper>()
            .AddLoggers();
}