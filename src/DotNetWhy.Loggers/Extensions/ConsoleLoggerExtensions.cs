namespace DotNetWhy.Loggers.Extensions;

internal static class ConsoleLoggerExtensions
{
    public static string SearchedPackageName { get; private set; }

    public static void Set(this Color color) =>
        System.Console.ForegroundColor = Enum.Parse<ConsoleColor>(color.ToString());

    public static void Set(this Encoding encoding) =>
        System.Console.OutputEncoding = encoding;

    public static void SetAsSearchedPackageName(this string packageName) =>
        SearchedPackageName = packageName;

    public static void SetLabelWidth(this Solution solution) => 
        ConsoleLoggerConstants.Widths.Label =
            new[]
            {
                solution.Name.Length,
                solution
                    .Projects
                    .Select(project => project.Name.Length).Max(),
                solution
                    .Projects
                    .SelectMany(project => project.Targets
                        .Select(target => target.Name.Length)).Max()
            }.Max()
            + ConsoleLoggerConstants.Widths.DoubleTab;

    public static string GetSolutionLabel(this Solution solution) =>
        GetLabel(
            ConsoleLoggerConstants.Prefixes.Solution,
            solution.Name,
            solution.DependencyCounter);

    public static string GetProjectLabel(this Project project, int solutionDependencyCounter) =>
        GetLabel(
            ConsoleLoggerConstants.Prefixes.Project,
            project.Name,
            solutionDependencyCounter,
            project.DependencyCounter);

    public static string GetTargetLabel(this Target target, int projectDependencyCounter) =>
        GetLabel(
            ConsoleLoggerConstants.Prefixes.Target,
            target.Name,
            projectDependencyCounter,
            target.DependencyCounter);

    private static string GetLabel(
        string prefix,
        string name,
        int dependencyCounter,
        int? childDependencyCounter = null) =>
        $"{prefix} {$"{name}".PadRight(ConsoleLoggerConstants.Widths.Label)} {$"[{(childDependencyCounter.HasValue ? $"{childDependencyCounter}/" : string.Empty)}{dependencyCounter}]".PadLeft(ConsoleLoggerConstants.Widths.QuadrupleTab)}";
}