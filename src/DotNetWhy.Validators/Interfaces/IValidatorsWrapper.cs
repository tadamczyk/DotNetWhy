namespace DotNetWhy.Validators;

public interface IValidatorsWrapper
{
    IValidatorsWrapper AddInitializedDependenciesValidator<T>(T service);

    IValidatorsWrapper AddNotNullOrEmptyValidator<T>(
        IEnumerable<T> parameter,
        string parameterName = null);

    IValidatorsWrapper Add(BaseValidator validator);

    void ValidateAndExecute(
        Action<IValidatorsWrapper> validators,
        Action onSuccess,
        Action<IEnumerable<string>> onFailure = null);
}