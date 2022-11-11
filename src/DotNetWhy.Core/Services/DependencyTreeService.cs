namespace DotNetWhy.Core.Services;

internal class DependencyTreeService : IDependencyTreeService
{
    private readonly IDependencyGraphConverter _converter;
    private readonly IDependencyGraphProvider _provider;
    private readonly ILockFilesGenerator _lockFilesGenerator;

    public DependencyTreeService(
        IDependencyGraphConverter converter,
        IDependencyGraphProvider provider,
        ILockFilesGenerator lockFilesGenerator)
    {
        _converter = converter;
        _provider = provider;
        _lockFilesGenerator = lockFilesGenerator;
    }

    public Solution GetDependencyTreeByPackageName(
        string workingDirectory,
        string packageName)
    {
        var solutionName = Path.GetFileName(workingDirectory) ?? workingDirectory;

        DependencyGraphSpec solutionDependencyGraph = default;
        Parallel.Invoke(
            () => { solutionDependencyGraph = _provider.Get(workingDirectory); },
            () => _lockFilesGenerator.Generate(workingDirectory)
        );

        if (solutionDependencyGraph?.Projects?.IsNullOrEmpty() ?? true)
        {
            return new Solution(solutionName);
        }

        var solution = _converter.Convert(solutionDependencyGraph, solutionName, packageName);

        return solution;
    }
}