namespace DotNetWhy.Services.Validators;

internal sealed record DirectoryProjectsValidator(IFileSystem FileSystem)
    : BaseValidator
{
    protected override bool IsValid =>
        FileSystem
            .Directory
            .GetFiles(GetDirectory())
            .Any(file => file.EndsWith(".sln") || file.EndsWith(".csproj"));

    protected override string ErrorMessage =>
        $"Directory {GetDirectory()} does not contain any C# project.";

    private string GetDirectory() =>
        FileSystem
            .Directory
            .GetCurrentDirectory();
}