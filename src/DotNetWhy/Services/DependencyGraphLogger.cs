namespace DotNetWhy.Services;

internal interface IDependencyGraphLogger
{
    void Log(SolutionDependencyGraph solutionDependencyGraph, string packageName);
}

internal class DependencyGraphLogger : IDependencyGraphLogger
{
    public void Log(SolutionDependencyGraph solutionDependencyGraph, string packageName)
    {
                var maxWidth = Console.WindowWidth - 4;
        if (solutionDependencyGraph?.ProjectsDependencyGraphs?.Any() ?? false)
        {
            var width = solutionDependencyGraph.ProjectsDependencyGraphs.Max(p => p.Name.Length) + 2;
            Console.OutputEncoding = Encoding.UTF8;
            foreach (var project in solutionDependencyGraph.ProjectsDependencyGraphs)
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
                            if (dependency.Contains(packageName))
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
    }
}