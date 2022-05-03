namespace DotNetWhy.Validators;

public interface IValidatorsManager
{
    void AddServiceDependenciesValidatorFor<T>(T service);
    void AddNullOrEmptyValidator<T>(IEnumerable<T> @object, string objectName = null);
    void Add(BaseValidator validator);
    void Validate(Action<IValidatorsManager> validators,
        Action onSuccess,
        Action<IEnumerable<string>> onFailure);
}