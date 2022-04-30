namespace DotNetWhy.Services.Validators;

internal sealed record ArgumentsValidator(IEnumerable<string> Arguments) : BaseValidator
{
    protected override bool ValidCondition =>
        Arguments is not null && Arguments.Any();

    protected override string ErrorMessage =>
        "Package name was not specified.";
}