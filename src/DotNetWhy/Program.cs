namespace DotNetWhy;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var serviceProvider = IServiceProviderExtension.GetServiceProvider();
        var fileSystem = serviceProvider.GetService<IFileSystem>();
        var dependencyTreeService = serviceProvider.GetService<IDependencyGraphService>();
        var directory = fileSystem?.Directory.GetCurrentDirectory();

        Console.WriteLine($"Analyzing projects from {directory} directory...\n");

        var dependencyTreeForPackage = dependencyTreeService
            ?.GetDependencyGraphByPackageName(directory, args[0]);

        if (dependencyTreeForPackage?.ProjectsDependencyGraphs?.Any() ?? false)
        {
            foreach (var project in dependencyTreeForPackage.ProjectsDependencyGraphs)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(project.Name);
                foreach (var target in project.TargetsDependencyGraphs)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($" [{target.Name}]");
                    foreach (var path in target.DependenciesPaths)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine($"  {string.Join(" -> ", path.Select(p => $"{p.Name} ({p.Version})"))}");
                    }
                }

                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
}