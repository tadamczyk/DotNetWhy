namespace DotNetWhy.Validators;

internal sealed record NotNullOrEmptyValidator<T>(
        IEnumerable<T> Parameter,
        string ParameterName = null)
    : BaseValidator
{
    protected internal override bool IsValid =>
        Parameter is not null
        && Parameter.Any();

    protected internal override string ErrorMessage =>
        $"Parameter{(string.IsNullOrEmpty(ParameterName) ? string.Empty : $" '{ParameterName}'")} is null or empty.";
}