namespace DotNetWhy.Services.Services;

internal class DependencyGraphLogger : IDependencyGraphLogger
{
    private record Width(int Max, int Label);

    private static class Widths
    {
        public static int Tab => 2;
        public static int DoubleTab => 2 * Tab;
        public static int TripleTab => 3 * Tab;
        public static int QuadrupleTab => 4 * Tab;
    }

    private static class Prefixes
    {
        public const string Solution = "\u1409";
        public const string Project = "\u1405";
        public const string Target = "\u1433";
    }

    public DependencyGraphLogger()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    public void Log(
        SolutionDependencyGraph solutionDependencyGraph,
        string packageName)
    {
        if (solutionDependencyGraph is null || !solutionDependencyGraph.ProjectsDependencyGraphs.Any())
        {
            LogLine($"Package {packageName} usage not found.");
            return;
        }

        var width = new Width(Console.WindowWidth - Widths.DoubleTab, GetLabelWidth(solutionDependencyGraph));

        var dependenciesPathsCountForSolution = GetDependenciesPathsCountForSolution(solutionDependencyGraph);
        LogLine(
            GetLabel(
                Prefixes.Solution,
                solutionDependencyGraph.Name,
                width.Label,
                dependenciesPathsCountForSolution),
            ConsoleColor.DarkCyan);

        foreach (var project in solutionDependencyGraph.ProjectsDependencyGraphs)
        {
            var dependenciesPathsCountForProject = GetDependenciesPathsCountForProject(project);
            LogLine(
                GetLabel(
                    Prefixes.Project,
                    project.Name,
                    width.Label,
                    dependenciesPathsCountForSolution,
                    dependenciesPathsCountForProject),
                ConsoleColor.Green);

            foreach (var target in project.TargetsDependencyGraphs)
            {
                var dependenciesPathsCountForTarget = target.DependenciesPaths.Count;
                LogLine(
                    GetLabel(
                        Prefixes.Target,
                        target.Name,
                        width.Label,
                        dependenciesPathsCountForProject,
                        dependenciesPathsCountForTarget),
                    ConsoleColor.DarkGreen);

                for (var index = 1; index <= target.DependenciesPaths.Count; index++)
                {
                    LogDependencyPath(
                        target.DependenciesPaths.ElementAt(index - 1),
                        packageName,
                        width.Max,
                        index);
                }
            }

            LogLine();
        }
    }

    private static int GetLabelWidth(SolutionDependencyGraph solutionDependencyGraph) =>
        new[]
        {
            solutionDependencyGraph.Name.Length,
            solutionDependencyGraph
                .ProjectsDependencyGraphs
                .Select(p => p.Name.Length)
                .Max(),
            solutionDependencyGraph
                .ProjectsDependencyGraphs
                .SelectMany(p => p.TargetsDependencyGraphs
                    .Select(t => t.Name.Length))
                .Max()
        }.Max() + Widths.DoubleTab;

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

    private static string GetLabel(
        string prefix,
        string name,
        int width,
        int all,
        int? count = null) =>
        $"{prefix} {$"{name}".PadRight(width)} {$"[{(count.HasValue ? $"{count}/" : string.Empty)}{all}]".PadLeft(Widths.QuadrupleTab)}";

    private static void LogDependencyPath(
        IReadOnlyList<DependenciesPath> dependenciesPath,
        string packageName,
        int maxWidth,
        int iterator)
    {
        Log($"{iterator}.".PadRight(Widths.DoubleTab));

        for (int index = 0, widthIterator = Widths.Tab; index < dependenciesPath.Count; index++)
        {
            var isLastDependency = index == dependenciesPath.Count - 1;
            var width = isLastDependency ? 0 : Widths.DoubleTab;
            var dependencyLabel = $"{dependenciesPath[index].Name} ({dependenciesPath[index].Version})";
            var isInline = widthIterator + dependencyLabel.Length + width <= maxWidth;
            widthIterator = dependencyLabel.Length + width + (isInline ? widthIterator : Widths.TripleTab);

            Log(isInline ? dependencyLabel : $"\n      {dependencyLabel}",
                dependencyLabel.Contains(packageName) ? ConsoleColor.Red : null);
            Log($"{(isLastDependency ? string.Empty : " -> ")}");
        }

        LogLine();
    }

    private static void Log(string text = "", ConsoleColor? color = null)
    {
        if (color.HasValue) Console.ForegroundColor = color.Value;
        Console.Write(text);
        Console.ResetColor();
    }

    private static void LogLine(string text = "", ConsoleColor? color = null)
    {
        if (color.HasValue) Console.ForegroundColor = color.Value;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}