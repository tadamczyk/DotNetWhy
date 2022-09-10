namespace DotNetWhy.Validators.Wrappers;

internal class ValidationWrapper : IValidationWrapper
{
    private readonly IList<BaseValidator> _validators = new List<BaseValidator>();

    public IValidationWrapper AddInitializedDependenciesValidator<T>(T service)
    {
        _validators.Add(new InitializedDependenciesValidator(service));
        return this;
    }

    public IValidationWrapper AddNotNullOrEmptyValidator<T>(
        IEnumerable<T> parameter,
        string parameterName = null)
    {
        _validators.Add(new NotNullOrEmptyValidator<T>(parameter, parameterName));
        return this;
    }

    public IValidationWrapper Add(BaseValidator validator)
    {
        _validators.Add(validator);
        return this;
    }

    public void ValidateAndExecute(
        Action<IValidationWrapper> validators,
        Action onSuccess,
        Action<IEnumerable<string>> onFailure)
    {
        try
        {
            validators(this);

            foreach (var validator in _validators)
            {
                if (validator.IsValid) continue;
                onFailure(new[] {validator.ErrorMessage});

                return;
            }

            onSuccess();
        }
        catch (Exception exception)
        {
            onFailure(new[] {exception.Message});
        }
    }
}