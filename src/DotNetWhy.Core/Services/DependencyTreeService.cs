namespace DotNetWhy.Core.Services;

internal class DependencyTreeService : IDependencyTreeService
{
    private readonly IDependencyGraphConverter _converter;
    private readonly IDependencyGraphProvider _provider;

    public DependencyTreeService(
        IDependencyGraphConverter converter,
        IDependencyGraphProvider provider)
    {
        _converter = converter;
        _provider = provider;
    }

    public Solution GetDependencyTreeByPackageName(
        string workingDirectory,
        string packageName)
    {
        var solutionName = Path.GetFileName(workingDirectory) ?? string.Empty;
        var solutionDependencyGraph = _provider.Get(workingDirectory);
        if (solutionDependencyGraph.Projects.IsNullOrEmpty())
        {
            return new Solution(solutionName);
        }

        var solution = _converter.Convert(solutionDependencyGraph, solutionName, packageName);

        return solution;
    }
}