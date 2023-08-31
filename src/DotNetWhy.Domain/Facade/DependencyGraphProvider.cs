namespace DotNetWhy.Domain.Facade;

public interface IDependencyGraphProvider
{
    Solution Get(
        string workingDirectory,
        string packageName,
        string packageVersion);
}

internal sealed class DependencyGraphProvider : IDependencyGraphProvider
{
    private readonly IMediator _mediator;

    public DependencyGraphProvider(IMediator mediator) =>
        _mediator = mediator;

    public Solution Get(
        string workingDirectory,
        string packageName,
        string packageVersion)
    {
        var solutionName = GetSolutionName(workingDirectory);
        var solutionDependencyGraphSpec = GetSolutionDependencyGraphSpec(workingDirectory);
        var solution = GetSolution(solutionDependencyGraphSpec, solutionName, packageName, packageVersion);

        return solution;
    }

    private static string GetSolutionName(string workingDirectory) =>
        Path.GetFileName(workingDirectory) ?? workingDirectory;

    private DependencyGraphSpec GetSolutionDependencyGraphSpec(string workingDirectory)
    {
        DependencyGraphSpec solutionDependencyGraphSpec = null;

        Parallel.Invoke(
            () =>
                solutionDependencyGraphSpec =
                    _mediator.Send<GetDependencyGraphSpecCommand, DependencyGraphSpec>(
                        new GetDependencyGraphSpecCommand(workingDirectory)),
            () =>
                _mediator.Send(
                    new CreateLockFilesCommand(workingDirectory))
        );

        return solutionDependencyGraphSpec;
    }

    private Solution GetSolution(
        DependencyGraphSpec dependencyGraphSpec,
        string solutionName,
        string packageName,
        string packageVersion) =>
        dependencyGraphSpec?.Projects?.IsNullOrEmpty() ?? true
            ? new Solution(solutionName)
            : _mediator.Send<ConvertDependencyGraphSpecCommand, Solution>(
                new ConvertDependencyGraphSpecCommand(
                    dependencyGraphSpec,
                    solutionName,
                    packageName,
                    packageVersion));
}