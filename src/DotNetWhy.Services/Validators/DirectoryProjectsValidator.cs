namespace DotNetWhy.Services.Validators;

internal sealed record DirectoryProjectsValidator(IFileSystem FileSystem) : BaseValidator
{
    protected override bool IsValid =>
        FileSystem.Directory.GetFiles(_directory).Any(file => file.EndsWith(".sln") || file.EndsWith(".csproj"));

    protected override string ErrorMessage =>
        $"Directory {_directory} does not contain any C# project.";

    private readonly string _directory = FileSystem.Directory.GetCurrentDirectory();
}