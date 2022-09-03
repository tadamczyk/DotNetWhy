namespace DotNetWhy.Core.Services;

internal class DependencyGraphProvider : IDependencyGraphProvider
{
    private const int MaxRetryLimit = 3;
    private readonly IDependencyGraphSourceProvider _dependencyGraphSourceProvider;
    private int _retryCounter;

    public DependencyGraphProvider(IDependencyGraphSourceProvider dependencyGraphSourceProvider)
    {
        _dependencyGraphSourceProvider = dependencyGraphSourceProvider;
    }

    public DependencyGraphSpec Get(string workingDirectory)
    {
        try
        {
            var dependencyGraphSource = _dependencyGraphSourceProvider.Get();
            var dotNetGenerateGraphFileResult = DotNetRunner.GenerateGraphFile(workingDirectory, dependencyGraphSource);

            return dotNetGenerateGraphFileResult.IsSuccess
                ? DependencyGraphSpec.Load(dependencyGraphSource)
                : throw new GenerateGraphFileFailedException(workingDirectory);
        }
        catch (Exception exception) when (exception is not GenerateGraphFileFailedException)
        {
            if (_retryCounter++ < MaxRetryLimit)
                return Get(workingDirectory);
            
            throw new GenerateGraphFileFailedException(workingDirectory);
        }
    }
}