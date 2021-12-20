namespace DotNetWhy.Services;

internal class DependencyGraphLogger : IDependencyGraphLogger
{
    private const string ProjectPrefix = "\u25B6";
    private const string TargetPrefix = "  ";

    public void Log(SolutionDependencyGraph solutionDependencyGraph, string packageName)
    {
        if (!solutionDependencyGraph?.ProjectsDependencyGraphs?.Any() ?? false)
        {
            Console.WriteLine($"Package {packageName} usage not found.");
            return;
        }

        Console.OutputEncoding = Encoding.UTF8;

        var maxOutputWidth = Console.WindowWidth - 4;
        var labelWidth = GetLabelWidth(solutionDependencyGraph);

        foreach (var project in solutionDependencyGraph.ProjectsDependencyGraphs)
        {
            var dependenciesPathsCountForProject = GetDependenciesPathsCountForProject(project);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(GetProjectLabel(project, dependenciesPathsCountForProject, labelWidth));

            foreach (var target in project.TargetsDependencyGraphs)
            {
                var dependenciesPathsCountForTarget = target.DependenciesPaths.Count;

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(GetTargetLabel(target, dependenciesPathsCountForProject, dependenciesPathsCountForTarget, labelWidth));
                Console.ResetColor();

                foreach (var dependenciesPath in target.DependenciesPaths)
                {
                    LogDependencyPath(dependenciesPath, packageName, maxOutputWidth);
                }
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }

    private static int GetLabelWidth(SolutionDependencyGraph solutionDependencyGraph) =>
        solutionDependencyGraph
            .ProjectsDependencyGraphs
            .Select(p => p.Name.Length)
            .Concat(solutionDependencyGraph
                .ProjectsDependencyGraphs
                .SelectMany(p => p.TargetsDependencyGraphs
                    .Select(t => t.Name.Length)))
            .Max() + 2;

    private static int GetDependenciesPathsCountForProject(ProjectDependencyGraph projectDependencyGraph) =>
        projectDependencyGraph
            .TargetsDependencyGraphs
            .Select(t => t.DependenciesPaths.Count)
            .Sum();

    private static string GetProjectLabel(ProjectDependencyGraph projectDependencyGraph, int dependenciesPathsCountForProject, int nameWidth) =>
        $"{ProjectPrefix} {projectDependencyGraph.Name.PadRight(nameWidth + dependenciesPathsCountForProject.ToString().Length + 1)} [{dependenciesPathsCountForProject}]";

    private static string GetTargetLabel(TargetDependencyGraph target, int dependenciesPathsCountForProject, int dependenciesPathsCountForTarget, int nameWidth) =>
        $"{TargetPrefix}{$"[{target.Name}]".PadRight(nameWidth + (dependenciesPathsCountForProject.ToString().Length - dependenciesPathsCountForTarget.ToString().Length))} [{dependenciesPathsCountForTarget}/{dependenciesPathsCountForProject}]";

    private static void LogDependencyPath(DependenciesPath[] dependenciesPath, string packageName, int maxOutputWidth)
    {
        Console.Write(TargetPrefix);

        for (int index = 0, widthIterator = TargetPrefix.Length; index < dependenciesPath.Length; index++)
        {
            var isLastDependency = index == dependenciesPath.Length - 1;
            var dependencyLabel = $"{dependenciesPath[index].Name} ({dependenciesPath[index].Version})";

            if (dependencyLabel.Contains(packageName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            widthIterator += dependencyLabel.Length;
            var isInline = widthIterator <= maxOutputWidth;
            Console.Write(isInline ? dependencyLabel : $"\n{TargetPrefix}{TargetPrefix}{dependencyLabel}");
            Console.ResetColor();
            Console.Write($"{(isLastDependency ? string.Empty : " -> ")}");
            widthIterator = widthIterator + 4 <= maxOutputWidth ? widthIterator + 4 : 4;
        }

        Console.Write("\n");
    }
}