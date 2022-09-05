namespace DotNetWhy.Core.Services;

internal class DependencyGraphProvider : IDependencyGraphProvider
{
    private readonly IDependencyGraphSourceProvider _sourceProvider;
    private readonly Retry _retry;

    public DependencyGraphProvider(IDependencyGraphSourceProvider sourceProvider)
    {
        _sourceProvider = sourceProvider;
        _retry = new Retry();
    }

    public DependencyGraphSpec Get(string workingDirectory)
    {
        var dependencyGraphSource = _sourceProvider.Get();

        try
        {
            var dotNetGenerateGraphFileResult = DotNetRunner.GenerateGraphFile(workingDirectory, dependencyGraphSource);

            return dotNetGenerateGraphFileResult.IsSuccess
                ? DependencyGraphSpec.Load(dependencyGraphSource)
                : throw new GenerateGraphFileFailedException(workingDirectory);
        }
        catch (Exception exception) when (exception is not GenerateGraphFileFailedException)
        {
            return _retry.CanTryAgain()
                ? Get(workingDirectory)
                : throw new GenerateGraphFileFailedException(workingDirectory);
        }
    }
}