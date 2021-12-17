namespace DotNetWhy;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Restart();

        if (string.IsNullOrWhiteSpace(args[0]))
        {
            Console.WriteLine("Package name was not specified.");
            return;
        }
        
        var serviceProvider = IServiceProviderExtension.GetServiceProvider();
        var fileSystem = serviceProvider.GetService<IFileSystem>();
        var dependencyTreeService = serviceProvider.GetService<IDependencyGraphService>();
        var directory = fileSystem?.Directory.GetCurrentDirectory();

        if (!fileSystem.Directory.GetFiles(directory).Any(f => f.EndsWith(".sln") || f.EndsWith(".csproj")))
        {
            Console.WriteLine($"Directory {directory} does not contain any C# project.");
            return;
        }

        Console.WriteLine($"Analyzing project(s) from {directory} directory...\n");

        var dependencyTreeForPackage = dependencyTreeService
            ?.GetDependencyGraphByPackageName(directory, args[0]);

        var maxWidth = Console.WindowWidth - 4;
        if (dependencyTreeForPackage?.ProjectsDependencyGraphs?.Any() ?? false)
        {
            var width = dependencyTreeForPackage.ProjectsDependencyGraphs.Max(p => p.Name.Length) + 2;
            Console.OutputEncoding = Encoding.UTF8;
            foreach (var project in dependencyTreeForPackage.ProjectsDependencyGraphs)
            {
                var targetWidth = project.TargetsDependencyGraphs.Max(t => t.Name.Length) + 2;
                width = width > targetWidth ? width : targetWidth;
                var projectCount = project.TargetsDependencyGraphs
                    .Select(t => t.DependenciesPaths.Count).Sum();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\u25B6 {project.Name.PadRight(width + projectCount.ToString().Length + 1)} [{projectCount}]");
                foreach (var target in project.TargetsDependencyGraphs)
                {
                    var targetCount = target.DependenciesPaths.Count;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine($"  {$"[{target.Name}]".PadRight(width + (projectCount.ToString().Length - targetCount.ToString().Length))} [{targetCount}/{projectCount}]");
                    foreach (var path in target.DependenciesPaths)
                    {
                        Console.ResetColor();
                        Console.Write("  ");
                        for (int i = 0, j = 2; i < path.Length; i++)
                        {
                            var isLast = i == path.Length - 1;
                            var dependency = $"{path[i].Name} ({path[i].Version})";
                            j = j + dependency.Length + (isLast ? 0 : 4);
                            if (dependency.Contains(args[0]))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.Write(j <= maxWidth ? dependency : $"\n    {dependency}");
                            Console.ResetColor();
                            Console.Write($"{(isLast ? "" : " -> ")}");
                            j = j <= maxWidth ? j : 4;
                        }
                        Console.Write("\n");
                    }
                }
                
                Console.WriteLine();
            }

            Console.ResetColor();
        }
        stopWatch.Stop();
        
        Console.WriteLine($"Time elapsed: {stopWatch.Elapsed:hh\\:mm\\:ss\\.ff}");
    }
}