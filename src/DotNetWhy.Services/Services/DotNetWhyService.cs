namespace DotNetWhy.Services;

internal class DotNetWhyService : IDotNetWhyService
{
    private readonly IDependencyTreeLogger _logger;
    private readonly IDependencyTreeService _service;
    private readonly IValidator<IParameters> _validator;

    public DotNetWhyService(
        IDependencyTreeLogger logger,
        IDependencyTreeService service,
        IValidator<IParameters> validator)
    {
        _logger = logger;
        _service = service;
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

        var dependencyTree = _service.GetDependencyTreeByPackageName(
            parameters.WorkingDirectory,
            parameters.PackageName,
            parameters.PackageVersion);

        _logger.LogResults(
            dependencyTree,
            parameters.PackageName,
            parameters.PackageVersion);
    }
}