namespace DotNetWhy.Services.Services;

internal class DotNetWhyService : IDotNetWhyService
{
    private readonly IDependencyGraphLogger _logger;
    private readonly IDependencyGraphService _service;
    private readonly IFileSystem _fileSystem;
    private readonly IValidatorsManager _validatorsManager;

    public DotNetWhyService(IDependencyGraphLogger logger,
        IDependencyGraphService service,
        IFileSystem fileSystem,
        IValidatorsManager validatorsManager)
    {
        _logger = logger;
        _service = service;
        _fileSystem = fileSystem;
        _validatorsManager = validatorsManager;
    }

    public void Run(IReadOnlyCollection<string> arguments) =>
        StopwatchLogger.Log(() => _validatorsManager.Validate(validators =>
            {
                validators.AddServiceDependenciesValidatorFor(this);
                validators.AddNullOrEmptyValidator(arguments, "Arguments");
                validators.AddNullOrEmptyValidator(arguments.FirstOrDefault(), "Package name");
                validators.Add(new DirectoryProjectsValidator(_fileSystem));
            },
            () =>
            {
                var directory = _fileSystem?.Directory.GetCurrentDirectory();
                var packageName = arguments.First();

                Console.WriteLine($"Analyzing project(s) from {directory} directory...\n");
                var solutionDependencyGraph = _service.GetDependencyGraphByPackageName(directory, packageName);
                _logger.Log(solutionDependencyGraph, packageName);
            },
            errors =>
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            }));
}