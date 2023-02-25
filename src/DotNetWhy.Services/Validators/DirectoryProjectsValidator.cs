namespace DotNetWhy.Services.Validators;

internal sealed record DirectoryProjectsValidator : BaseValidator
{
    private const string ProjectFileExtension = ".csproj";
    private const string SolutionFileExtension = ".sln";

    protected override bool IsValid =>
        Directory
            .GetFiles(Environment.CurrentDirectory)
            .Any(file => file.EndsWith(SolutionFileExtension, StringComparison.InvariantCultureIgnoreCase)
                         || file.EndsWith(ProjectFileExtension, StringComparison.InvariantCultureIgnoreCase));

    protected override string ErrorMessage =>
        $"Directory {Environment.CurrentDirectory} does not contain any C# project.";
}