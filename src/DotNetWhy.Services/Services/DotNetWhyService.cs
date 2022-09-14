namespace DotNetWhy.Services;

internal class DotNetWhyService : IDotNetWhyService
{
    private readonly IDependencyTreeLogger _logger;
    private readonly IDependencyTreeService _service;
    private readonly IValidationWrapper _validationWrapper;

    public DotNetWhyService(
        IDependencyTreeLogger logger,
        IDependencyTreeService service,
        IValidationWrapper validationWrapper)
    {
        _logger = logger;
        _service = service;
        _validationWrapper = validationWrapper;
    }

    public void Run(IReadOnlyCollection<string> arguments)
    {
        _validationWrapper.ValidateAndExecute(
            SetValidators,
            GetDependencyTree,
            GetErrors);

        void SetValidators(IValidationWrapper validators)
        {
            validators
                .AddInitializedDependenciesValidator(this)
                .AddNotNullOrEmptyValidator(arguments, "Arguments")
                .AddNotNullOrEmptyValidator(arguments.FirstOrDefault(), "Package name")
                .Add(new DirectoryProjectsValidator());
        }

        void GetDependencyTree()
        {
            var workingDirectory = Environment.CurrentDirectory;
            var packageName = arguments.First();
            var dependencyTree = _service.GetDependencyTreeByPackageName(workingDirectory, packageName);
            _logger.LogResults(dependencyTree, packageName);
        }

        void GetErrors(IEnumerable<string> errors)
        {
            _logger.LogErrors(errors);
        }
    }
}