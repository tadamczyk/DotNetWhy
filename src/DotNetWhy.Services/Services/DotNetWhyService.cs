namespace DotNetWhy.Services.Services;

internal class DotNetWhyService : IDotNetWhyService
{
    private readonly IDependencyGraphLogger _logger;
    private readonly IDependencyGraphService _service;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger _consoleLogger;
    private readonly IValidatorsWrapper _validatorsWrapper;

    public DotNetWhyService(
        IDependencyGraphLogger logger,
        IDependencyGraphService service,
        IFileSystem fileSystem,
        ILogger consoleLogger,
        IValidatorsWrapper validatorsWrapper)
    {
        _logger = logger;
        _service = service;
        _fileSystem = fileSystem;
        _consoleLogger = consoleLogger;
        _validatorsWrapper = validatorsWrapper;
    }

    public void Run(IReadOnlyCollection<string> arguments)
    {
        _validatorsWrapper.ValidateAndExecute(
            SetValidators,
            GetDependencyTree);

        void SetValidators(IValidatorsWrapper validators)
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

            _consoleLogger.LogLine($"Analyzing project(s) from {directory} directory...");
            var solutionDependencyGraph = _service.GetConvertedDependencyGraphByPackageName(directory, packageName);
            _logger.Log(solutionDependencyGraph, packageName);
        }
    }
}