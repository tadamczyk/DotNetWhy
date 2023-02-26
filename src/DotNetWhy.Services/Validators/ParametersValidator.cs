namespace DotNetWhy.Services.Validators;

internal class ParametersValidator : AbstractValidator<IParameters>
{
    private const string ProjectFileExtension = ".csproj";
    private const string SolutionFileExtension = ".sln";

    public ParametersValidator()
    {
        RuleFor(parameters => parameters.PackageName)
            .Must(packageName => !string.IsNullOrEmpty(packageName))
            .WithMessage(
                "Package name not specified. Please run command once again specifying package name - 'dotnet why PACKAGE_NAME'.");

        RuleFor(parameters => parameters.WorkingDirectory)
            .Must(workingDirectory =>
                Directory
                    .GetFiles(workingDirectory)
                    .Any(file =>
                        file.EndsWith(SolutionFileExtension, StringComparison.InvariantCultureIgnoreCase)
                        || file.EndsWith(ProjectFileExtension, StringComparison.InvariantCultureIgnoreCase)))
            .WithMessage(parameters => $"Directory {parameters.WorkingDirectory} does not contain any C# project.");
    }
}