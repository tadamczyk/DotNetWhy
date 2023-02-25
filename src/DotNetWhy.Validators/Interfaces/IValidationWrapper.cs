namespace DotNetWhy.Validators;

public interface IValidationWrapper
{
    IValidationWrapper AddInitializedDependenciesValidator<T>(T service);

    IValidationWrapper AddNotNullOrEmptyValidator<T>(
        IEnumerable<T> parameter,
        string parameterName = null);

    IValidationWrapper Add(BaseValidator validator);

    void ValidateAndExecute(
        Action<IValidationWrapper> validators,
        Action onSuccess,
        Action<IEnumerable<string>> onFailure);
}