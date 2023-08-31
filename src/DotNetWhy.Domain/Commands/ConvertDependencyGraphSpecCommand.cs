namespace DotNetWhy.Domain.Commands;

internal record struct ConvertDependencyGraphSpecCommand(
        DependencyGraphSpec DependencyGraphSpec,
        string SolutionName,
        string PackageName,
        string PackageVersion)
    : ICommand;