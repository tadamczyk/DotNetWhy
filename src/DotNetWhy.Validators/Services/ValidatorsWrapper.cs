namespace DotNetWhy.Validators.Services;

internal class ValidatorsWrapper : IValidatorsWrapper
{
    private readonly IList<BaseValidator> _validators = new List<BaseValidator>();

    public IValidatorsWrapper AddInitializedDependenciesValidator<T>(T service)
    {
        _validators.Add(new InitializedDependenciesValidator(service));
        return this;
    }

    public IValidatorsWrapper AddNotNullOrEmptyValidator<T>(
        IEnumerable<T> parameter,
        string parameterName = null)
    {
        _validators.Add(new NotNullOrEmptyValidator<T>(parameter, parameterName));
        return this;
    }

    public IValidatorsWrapper Add(BaseValidator validator)
    {
        _validators.Add(validator);
        return this;
    }

    public void ValidateAndExecute(
        Action<IValidatorsWrapper> validators,
        Action onSuccess,
        Action<IEnumerable<string>> onFailure = null)
    {
        try
        {
            validators(this);

            foreach (var validator in _validators)
            {
                if (!validator.IsValid)
                {
                    if (onFailure is null)
                        Console.WriteLine(validator.ErrorMessage);
                    else
                        onFailure(new[] {validator.ErrorMessage});

                    return;
                }
            }

            onSuccess();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}