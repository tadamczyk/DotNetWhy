namespace DotNetWhy.Validators.Validators;

internal sealed record ServiceDependenciesValidator(object Service) : BaseValidator
{
    protected internal override bool IsValid =>
        GetServicePrivateReadonlyFieldsValues()
            .All(value => value is not null);

    protected internal override string ErrorMessage =>
        "Service is not initialized properly.";

    private IEnumerable<object> GetServicePrivateReadonlyFieldsValues() =>
        GetServicePrivateReadonlyFields()
            .Select(field => field.GetValue(Service));

    private IEnumerable<FieldInfo> GetServicePrivateReadonlyFields() =>
        Service
            .GetType()
            .GetRuntimeFields()
            .Where(IsPrivateReadonlyInitializedByConstructorField);

    private bool IsPrivateReadonlyInitializedByConstructorField(FieldInfo field) =>
        field.IsPrivate
        && field.IsInitOnly
        && _serviceConstructorParametersTypes.Contains(field.FieldType);

    private readonly IEnumerable<Type> _serviceConstructorParametersTypes =
        Service
            .GetType()
            .GetConstructors()
            .Single()
            .GetParameters()
            .Select(parameter => parameter.ParameterType);
}