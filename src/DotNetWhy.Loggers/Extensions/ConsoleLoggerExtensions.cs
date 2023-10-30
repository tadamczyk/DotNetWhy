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

    public static void SetLabelWidth(this Node solution) =>
        ConsoleLoggerConstants.Widths.Label =
            new[]
            {
                solution.Name.Length,
                solution
                    .Nodes
                    .Select(node => node.Name.Length).Max(),
                solution
                    .Nodes
                    .SelectMany(node => node.Nodes
                        .Select(childNode => childNode.Name.Length)).Max()
            }.Max()
            + ConsoleLoggerConstants.Widths.DoubleTab;

    public static string GetSolutionLabel(this Node solution) =>
        GetLabel(
            ConsoleLoggerConstants.Prefixes.Solution,
            solution.Name,
            solution.LastNodesSum);

    public static string GetProjectLabel(this Node project, int solutionDependencyCounter) =>
        GetLabel(
            ConsoleLoggerConstants.Prefixes.Project,
            project.Name,
            solutionDependencyCounter,
            project.LastNodesSum);

    public static string GetTargetLabel(this Node target, int projectDependencyCounter) =>
        GetLabel(
            ConsoleLoggerConstants.Prefixes.Target,
            target.Name,
            projectDependencyCounter,
            target.LastNodesSum);

    private static string GetLabel(
        string prefix,
        string name,
        int dependencyCounter,
        int? childDependencyCounter = null) =>
        $"{prefix} {$"{name}".PadRight(ConsoleLoggerConstants.Widths.Label)} {$"[{(childDependencyCounter.HasValue ? $"{childDependencyCounter}/" : string.Empty)}{dependencyCounter}]".PadLeft(ConsoleLoggerConstants.Widths.QuadrupleTab)}";
}