namespace DotNetWhy.Services.Services;

internal class DotNetWhyService : IDotNetWhyService
{
    private readonly IDependencyGraphLogger _logger;
    private readonly IDependencyGraphService _service;
    private readonly IFileSystem _fileSystem;

    public DotNetWhyService(IDependencyGraphLogger logger,
        IDependencyGraphService service,
        IFileSystem fileSystem)
    {
        _logger = logger;
        _service = service;
        _fileSystem = fileSystem;
    }

    public void Run(string[] arguments)
    {
        var stopWatch = Stopwatch.StartNew();

        var directory = _fileSystem?.Directory.GetCurrentDirectory();

        var validators = new BaseValidator[]
        {
            new ServicesValidator(this),
            new ArgumentsValidator(arguments),
            new PackageNameValidator(arguments[0]),
            new DirectoryProjectsValidator(_fileSystem, directory)
        };

        foreach (var validator in validators)
        {
            if (validator.IsFailure)
            {
                validator.LogError();
                return;
            }
        }

        Console.WriteLine($"Analyzing project(s) from {directory} directory...\n");

        var packageName = arguments[0];
        var solutionDependencyGraph = _service.GetDependencyGraphByPackageName(directory, packageName);
        _logger.Log(solutionDependencyGraph, packageName);

        stopWatch.Stop();

        Console.WriteLine($"Time elapsed: {stopWatch.Elapsed:hh\\:mm\\:ss\\.ff}");
    }
}