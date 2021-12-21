namespace DotNetWhy.Services;

internal class DependencyGraphLogger : IDependencyGraphLogger
{
    private const string SolutionPrefix = "\u1409";
    private const string ProjectPrefix = "\u1405";
    private const string TargetPrefix = "\u1433";

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
        var dependenciesPathsCountForSolution = GetDependenciesPathsCountForSolution(solutionDependencyGraph);

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(GetSolutionLabel(solutionDependencyGraph, dependenciesPathsCountForSolution, labelWidth));

        foreach (var project in solutionDependencyGraph.ProjectsDependencyGraphs)
        {
            var dependenciesPathsCountForProject = GetDependenciesPathsCountForProject(project);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(GetProjectLabel(project, dependenciesPathsCountForSolution, dependenciesPathsCountForProject, labelWidth));

            foreach (var target in project.TargetsDependencyGraphs)
            {
                var dependenciesPathsCountForTarget = target.DependenciesPaths.Count;

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(GetTargetLabel(target, dependenciesPathsCountForProject, dependenciesPathsCountForTarget, labelWidth));
                Console.ResetColor();

                for (var index = 1; index <= target.DependenciesPaths.Count; index++)
                {
                    LogDependencyPath(target.DependenciesPaths.ElementAt(index - 1), packageName, maxOutputWidth, index);
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
            .Concat(new[] {solutionDependencyGraph.Name.Length})
            .Max() + 8;

    private static int GetDependenciesPathsCountForSolution(SolutionDependencyGraph solutionDependencyGraph) =>
        solutionDependencyGraph
            .ProjectsDependencyGraphs
            .Select(GetDependenciesPathsCountForProject)
            .Sum();

    private static int GetDependenciesPathsCountForProject(ProjectDependencyGraph projectDependencyGraph) =>
        projectDependencyGraph
            .TargetsDependencyGraphs
            .Select(t => t.DependenciesPaths.Count)
            .Sum();

    private static string GetSolutionLabel(SolutionDependencyGraph solutionDependencyGraph, int dependenciesPathsCountForSolution, int nameWidth) =>
        $"{SolutionPrefix} {solutionDependencyGraph.Name.PadRight(nameWidth + dependenciesPathsCountForSolution.ToString().Length + 2)} [{dependenciesPathsCountForSolution}]";

    private static string GetProjectLabel(ProjectDependencyGraph projectDependencyGraph, int dependenciesPathsCountForSolution, int dependenciesPathsCountForProject, int nameWidth) =>
        $"{ProjectPrefix} {projectDependencyGraph.Name.PadRight(nameWidth + dependenciesPathsCountForSolution.ToString().Length - dependenciesPathsCountForProject.ToString().Length + 1)} [{dependenciesPathsCountForProject}/{dependenciesPathsCountForSolution}]";

    private static string GetTargetLabel(TargetDependencyGraph target, int dependenciesPathsCountForProject, int dependenciesPathsCountForTarget, int nameWidth) =>
        $"{TargetPrefix} {$"[{target.Name}]".PadRight(nameWidth + dependenciesPathsCountForProject.ToString().Length - dependenciesPathsCountForTarget.ToString().Length + 1)} [{dependenciesPathsCountForTarget}/{dependenciesPathsCountForProject}]";

    private static void LogDependencyPath(DependenciesPath[] dependenciesPath, string packageName, int maxOutputWidth, int iterator)
    {
        Console.Write($"{iterator}.".PadRight(4));

        for (int index = 0, widthIterator = 2; index < dependenciesPath.Length; index++)
        {
            var isLastDependency = index == dependenciesPath.Length - 1;
            var dependencyLabel = $"{dependenciesPath[index].Name} ({dependenciesPath[index].Version})";

            if (dependencyLabel.Contains(packageName)) Console.ForegroundColor = ConsoleColor.Red;

            var isInline = widthIterator + dependencyLabel.Length + (isLastDependency ? 0 : 4) <= maxOutputWidth;
            Console.Write(isInline ? dependencyLabel : $"\n      {dependencyLabel}");
            Console.ResetColor();
            Console.Write($"{(isLastDependency ? string.Empty : " -> ")}");
            widthIterator = isInline
                ? widthIterator + dependencyLabel.Length + (isLastDependency ? 0 : 4)
                : 6 + dependencyLabel.Length + (isLastDependency ? 0 : 4);
        }

        Console.Write("\n");
    }
}