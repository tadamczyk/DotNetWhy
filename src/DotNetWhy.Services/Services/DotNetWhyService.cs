namespace DotNetWhy.Services;

internal class DotNetWhyService : IDotNetWhyService
{
    private readonly IDependencyTreeLogger _logger;
    private readonly IDependencyTreeProvider _provider;
    private readonly IValidator<IParameters> _validator;

    public DotNetWhyService(
        IDependencyTreeLogger logger,
        IDependencyTreeProvider provider,
        IValidator<IParameters> validator)
    {
        _logger = logger;
        _provider = provider;
        _validator = validator;
    }

    public async Task RunAsync(IParameters parameters)
    {
        var validationResult = await _validator.ValidateAsync(parameters);

        if (!validationResult.IsValid)
        {
            _logger.LogErrors(validationResult.Errors.Select(error => error.ErrorMessage).ToHashSet());
            return;
        }

        var dependencyGraph = await _provider.GetAsync(
            new DependencyTreeParameters(
                parameters.WorkingDirectory,
                parameters.PackageName,
                parameters.PackageVersion));

        _logger.LogResults(
            dependencyGraph,
            parameters.PackageName,
            parameters.PackageVersion);
    }
}