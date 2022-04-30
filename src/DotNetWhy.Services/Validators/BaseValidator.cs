namespace DotNetWhy.Services.Validators;

internal abstract record BaseValidator
{
    protected abstract bool ValidCondition { get; }
    protected abstract string ErrorMessage { get; }

    internal bool IsFailure => !ValidCondition;
    internal void LogError() => Console.WriteLine(ErrorMessage);
}