namespace DotNetWhy.Core.Validations;

internal sealed class WorkingDirectoryValidator
(
    string workingDirectory
) : IResultHandler
{
    private const StringComparison StringComparisonType = StringComparison.InvariantCultureIgnoreCase;
    private const string CSharpProjectFileExtension = ".csproj";
    private const string FSharpProjectFileExtension = ".fsproj";
    private const string SolutionFileExtension = ".sln";

    public Result Handle()
    {
        var workingDirectoryFiles = Directory.GetFiles(workingDirectory);
        var workingDirectoryContainsCSharpProject = workingDirectoryFiles
            .Any(file => file.EndsWith(CSharpProjectFileExtension, StringComparisonType));
        var workingDirectoryContainsFSharpProject = workingDirectoryFiles
            .Any(file => file.EndsWith(FSharpProjectFileExtension, StringComparisonType));
        var workingDirectoryContainsSolution = workingDirectoryFiles
            .Any(file => file.EndsWith(SolutionFileExtension, StringComparisonType));

        return workingDirectoryContainsCSharpProject ||
               workingDirectoryContainsFSharpProject ||
               workingDirectoryContainsSolution
            ? Result.Success()
            : Result.Failure(Errors.DirectoryWithoutAnyProject(workingDirectory));
    }

    private static class Errors
    {
        public static string DirectoryWithoutAnyProject(string workingDirectory) =>
            $"Directory {workingDirectory} does not contain any C#/F# project.";
    }
}