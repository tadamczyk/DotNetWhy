namespace DotNetWhy.Services.Validators;

internal sealed record DirectoryProjectsValidator(IFileSystem FileSystem, string Directory) : BaseValidator
{
    protected override bool ValidCondition =>
        FileSystem.Directory.GetFiles(Directory).Any(file => file.EndsWith(".sln") || file.EndsWith(".csproj"));

    protected override string ErrorMessage =>
        $"Directory {Directory} does not contain any C# project.";
}