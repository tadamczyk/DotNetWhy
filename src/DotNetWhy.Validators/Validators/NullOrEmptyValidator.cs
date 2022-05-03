namespace DotNetWhy.Validators.Validators;

internal sealed record NullOrEmptyValidator<T>(IEnumerable<T> Object, string ObjectName = null) : BaseValidator
{
    protected internal override bool IsValid =>
        Object is not null && Object.Any();

    protected internal override string ErrorMessage =>
        $"{(string.IsNullOrEmpty(ObjectName) ? "Object" : ObjectName)} is null or empty.";
}