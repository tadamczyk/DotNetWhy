namespace DotNetWhy.Core.Validations;

internal sealed class WorkingDirectoryValidator
(
    string workingDirectory
) : IResultHandler
{
    private const StringComparison StringComparisonType = StringComparison.InvariantCultureIgnoreCase;
    private const string ProjectFileExtension = ".csproj";
    private const string SolutionFileExtension = ".sln";

    public Result Handle()
    {
        var workingDirectoryFiles = Directory.GetFiles(workingDirectory);
        var workingDirectoryContainsProject = workingDirectoryFiles
            .Any(file => file.EndsWith(ProjectFileExtension, StringComparisonType));
        var workingDirectoryContainsSolution = workingDirectoryFiles
            .Any(file => file.EndsWith(SolutionFileExtension, StringComparisonType));

        return workingDirectoryContainsProject || workingDirectoryContainsSolution
            ? Result.Success()
            : Result.Failure(Errors.DirectoryWithoutAnyProject(workingDirectory));
    }

    private static class Errors
    {
        public static string DirectoryWithoutAnyProject(string workingDirectory) =>
            $"Directory {workingDirectory} does not contain any C# project.";
    }
}