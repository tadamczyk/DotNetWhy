namespace DotNetWhy;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var stopWatch = Stopwatch.StartNew();

        var serviceProvider = IServiceProviderExtension.GetServiceProvider();
        var logger = serviceProvider.GetService<IDependencyGraphLogger>();
        var service = serviceProvider.GetService<IDependencyGraphService>();
        var fileSystem = serviceProvider.GetService<IFileSystem>();
        var directory = fileSystem?.Directory.GetCurrentDirectory();

        if (!ArgumentsValidator.IsValid(args)) return;
        if (!PackageNameValidator.IsValid(args[0])) return;
        if (!DependencyGraphLoggerValidator.IsValid(logger)) return;
        if (!DependencyGraphServiceValidator.IsValid(service)) return;
        if (!FileSystemValidator.IsValid(fileSystem)) return;
        if (!DirectoryProjectsValidator.IsValid(fileSystem, directory)) return;

        Console.WriteLine($"Analyzing project(s) from {directory} directory...\n");

        var packageName = args[0];
        var solutionDependencyGraph = service.GetDependencyGraphByPackageName(directory, packageName);
        logger.Log(solutionDependencyGraph, packageName);

        stopWatch.Stop();

        Console.WriteLine($"Time elapsed: {stopWatch.Elapsed:hh\\:mm\\:ss\\.ff}");
    }
}