namespace DotNetWhy.Validators;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddScoped<IValidatorsManager, ValidatorsManager>();
}