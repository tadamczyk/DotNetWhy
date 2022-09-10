namespace DotNetWhy.Services.Validators;

internal sealed record DirectoryProjectsValidator(IFileSystem FileSystem)
    : BaseValidator
{
    private const string ProjectFileExtension = ".csproj";
    private const string SolutionFileExtension = ".sln";

    protected override bool IsValid =>
        FileSystem
            .Directory
            .GetFiles(GetDirectory())
            .Any(file => file.EndsWith(SolutionFileExtension, StringComparison.InvariantCultureIgnoreCase)
                                || file.EndsWith(ProjectFileExtension, StringComparison.InvariantCultureIgnoreCase));

    protected override string ErrorMessage =>
        $"Directory {GetDirectory()} does not contain any C# project.";

    private string GetDirectory() =>
        FileSystem
            .Directory
            .GetCurrentDirectory();
}