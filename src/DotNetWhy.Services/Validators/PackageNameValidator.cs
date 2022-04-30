namespace DotNetWhy.Services.Validators;

internal sealed record PackageNameValidator(string PackageName) : BaseValidator
{
    protected override bool ValidCondition =>
        !string.IsNullOrWhiteSpace(PackageName);

    protected override string ErrorMessage =>
        "Package name was not specified.";
}