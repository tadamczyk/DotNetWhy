namespace DotNetWhy.Services.Validators;

internal sealed record ServicesValidator(object Service) : BaseValidator
{
    protected override bool ValidCondition =>
        GetServicePrivateReadonlyFieldsValues()
            .All(value => value is not null);

    protected override string ErrorMessage =>
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