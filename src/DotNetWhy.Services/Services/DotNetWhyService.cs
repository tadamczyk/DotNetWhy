namespace DotNetWhy.Services;

internal class DotNetWhyService : IDotNetWhyService
{
    private readonly IDependencyTreeLogger _logger;
    private readonly IDependencyTreeService _service;
    private readonly IFileSystem _fileSystem;
    private readonly IValidationWrapper _validationWrapper;

    public DotNetWhyService(
        IDependencyTreeLogger logger,
        IDependencyTreeService service,
        IFileSystem fileSystem,
        IValidationWrapper validationWrapper)
    {
        _logger = logger;
        _service = service;
        _fileSystem = fileSystem;
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
                .Add(new DirectoryProjectsValidator(_fileSystem));
        }

        void GetDependencyTree()
        {
            var directory = _fileSystem.Directory.GetCurrentDirectory().Trim();
            var packageName = arguments.First().Trim();

            _logger.LogStartMessage(directory);
            var dependencyTree = _service.GetDependencyTreeByPackageName(directory, packageName);
            _logger.LogResults(dependencyTree, packageName);
        }

        void GetErrors(IEnumerable<string> errors)
        {
            _logger.LogErrors(errors);
        }
    }
}