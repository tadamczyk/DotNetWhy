namespace DotNetWhy;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var stopWatch = Stopwatch.StartNew();

        var serviceProvider = IServiceProviderExtension.GetServiceProvider();
        var dependencyGraphService = serviceProvider?.GetService<IDependencyGraphService>();
        var fileSystem = serviceProvider?.GetService<IFileSystem>();
        var directory = fileSystem?.Directory.GetCurrentDirectory();

        if (!ArgumentsValidator.IsValid(args)) return;
        if (!PackageNameValidator.IsValid(args[0])) return;
        if (!DependencyGraphServiceValidator.IsValid(dependencyGraphService)) return;
        if (!FileSystemValidator.IsValid(fileSystem)) return;
        if (!DirectoryProjectsValidator.IsValid(fileSystem, directory)) return;

        Console.WriteLine($"Analyzing project(s) from {directory} directory...\n");

        var dependenciesPathsForPackage = dependencyGraphService.GetDependencyGraphByPackageName(directory, args[0]);
        var dependencyGraphLogger = serviceProvider.GetService<IDependencyGraphLogger>();
        dependencyGraphLogger.Log(dependenciesPathsForPackage, args[0]);

        stopWatch.Stop();

        Console.WriteLine($"Time elapsed: {stopWatch.Elapsed:hh\\:mm\\:ss\\.ff}");
    }
}