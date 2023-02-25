namespace DotNetWhy.Validators;

public abstract record BaseValidator
{
    protected internal abstract bool IsValid { get; }
    protected internal abstract string ErrorMessage { get; }
}