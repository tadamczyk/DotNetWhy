namespace DotNetWhy.Services;

internal class DotNetWhyService : IDotNetWhyService
{
    private readonly IDependencyTreeLogger _logger;
    private readonly IProvider _provider;

    public DotNetWhyService(
        IDependencyTreeLogger logger,
        IProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    public Task RunAsync(IParameters parameters)
    {
        var dependencyGraph = _provider.Get(new Request(parameters.PackageName)
        {
            PackageVersion = parameters.PackageVersion
        });

        if (!dependencyGraph.IsSuccess)
        {
            _logger.LogErrors(dependencyGraph.Errors.ToHashSet());
            return Task.CompletedTask;
        }

        _logger.LogResults(
            dependencyGraph.Node,
            parameters.PackageName,
            parameters.PackageVersion);

        return Task.CompletedTask;
    }
}