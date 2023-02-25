namespace DotNetWhy.Validators;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddSingleton<IValidationWrapper, ValidationWrapper>();
}