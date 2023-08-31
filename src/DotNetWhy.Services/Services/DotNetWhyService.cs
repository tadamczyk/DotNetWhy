namespace DotNetWhy.Services;

internal class DotNetWhyService : IDotNetWhyService
{
    private readonly IDependencyTreeLogger _logger;
    private readonly IDependencyGraphProvider _provider;
    private readonly IValidator<IParameters> _validator;

    public DotNetWhyService(
        IDependencyTreeLogger logger,
        IDependencyGraphProvider provider,
        IValidator<IParameters> validator)
    {
        _logger = logger;
        _provider = provider;
        _validator = validator;
    }

    public void Run(IParameters parameters)
    {
        var validationResult = _validator.Validate(parameters);

        if (!validationResult.IsValid)
        {
            _logger.LogErrors(validationResult.Errors.Select(error => error.ErrorMessage).ToHashSet());
            return;
        }

        var dependencyGraph = _provider.Get(
            parameters.WorkingDirectory,
            parameters.PackageName,
            parameters.PackageVersion);

        _logger.LogResults(
            dependencyGraph,
            parameters.PackageName,
            parameters.PackageVersion);
    }
}