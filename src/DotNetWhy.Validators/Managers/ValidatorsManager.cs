namespace DotNetWhy.Validators.Managers;

internal class ValidatorsManager : IValidatorsManager
{
    private readonly IList<BaseValidator> _validators;

    public ValidatorsManager() =>
        _validators = new List<BaseValidator>();

    public void AddServiceDependenciesValidatorFor<T>(T service) =>
        _validators.Add(new ServiceDependenciesValidator(service));

    public void AddNullOrEmptyValidator<T>(IEnumerable<T> parameter, string parameterName = null) =>
        _validators.Add(new NullOrEmptyValidator<T>(parameter, parameterName));

    public void Add(BaseValidator validator) =>
        _validators.Add(validator);

    public void Validate(Action<IValidatorsManager> validators,
        Action onSuccess,
        Action<IEnumerable<string>> onFailure)
    {
        validators(this);

        foreach (var validator in _validators)
        {
            if (!validator.IsValid)
            {
                onFailure(new[] {validator.ErrorMessage});
                return;
            }
        }

        onSuccess();
    }
}