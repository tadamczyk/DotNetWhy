namespace DotNetWhy.Core.Services;

internal class DependencyGraphProvider : IDependencyGraphProvider
{
    private readonly IDependencyGraphSourceProvider _dependencyGraphSourceProvider;

    public DependencyGraphProvider(IDependencyGraphSourceProvider dependencyGraphSourceProvider) =>
        _dependencyGraphSourceProvider = dependencyGraphSourceProvider;

    public DependencyGraphSpec Get(string workingDirectory)
    {
        var dependencyGraphSource = _dependencyGraphSourceProvider.Get();
        var dotNetGenerateGraphFileResult = DotNetRunner.GenerateGraphFile(workingDirectory, dependencyGraphSource);

        return dotNetGenerateGraphFileResult.IsSuccess
            ? DependencyGraphSpec.Load(dependencyGraphSource)
            : throw new GenerateGraphFileFailedException(workingDirectory);
    }
}