namespace DotNetWhy.Services.Services;

internal class DependencyGraphLogger : IDependencyGraphLogger
{
    private static int _iterator;

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

    private readonly ILogger _logger;

    public DependencyGraphLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void Log(
        Solution solution,
        string packageName)
    {
        if (!solution.HasProjects)
        {
            _logger.LogLine($"Package {packageName} usage not found.");
            return;
        }

        var (maxWidth, labelWidth) = new Width(_logger.Configuration.MaxWidth - Widths.DoubleTab, GetLabelWidth(solution));

        _logger.LogLine(
            GetLabel(
                Prefixes.Solution,
                solution.Name,
                labelWidth,
                solution.DependencyCounter),
            Color.DarkCyan);

        foreach (var project in solution.Projects)
        {
            _logger.LogLine(
                GetLabel(
                    Prefixes.Project,
                    project.Name,
                    labelWidth,
                    solution.DependencyCounter,
                    project.DependencyCounter),
                Color.Green);

            foreach (var target in project.Targets)
            {
                _logger.LogLine(
                    GetLabel(
                        Prefixes.Target,
                        target.Name,
                        labelWidth,
                        project.DependencyCounter,
                        target.DependencyCounter),
                    Color.DarkGreen);

                _iterator = 0;
                foreach (var dependency in target.Dependencies)
                {
                    Print(dependency);
                }
            }

            _logger.LogLine();
        }
    }

    private static int GetLabelWidth(Solution solution) =>
        new[]
        {
            solution.Name.Length,
            solution
                .Projects
                .Select(p => p.Name.Length)
                .Max(),
            solution
                .Projects
                .SelectMany(p => p.Targets
                    .Select(t => t.Name.Length))
                .Max()
        }.Max() + Widths.DoubleTab;

    private static string GetLabel(
        string prefix,
        string name,
        int width,
        int all,
        int? count = null) =>
        $"{prefix} {$"{name}".PadRight(width)} {$"[{(count.HasValue ? $"{count}/" : string.Empty)}{all}]".PadLeft(Widths.QuadrupleTab)}";

    private void Print(Dependency dependency, StringBuilder builder = null)
    {                        
        if (!dependency.HasDependencies)
        {
            _logger.Log($"{++_iterator}.".PadRight(Widths.DoubleTab));
            _logger.LogLine(builder?.ToString());
            return;
        }

        foreach (var childDependency in dependency.Dependencies)
        {
            builder = (builder ?? new StringBuilder(dependency.ToString()))
                .Append(' ')
                .Append("->")
                .Append(' ')
                .Append(childDependency)
                .Append(' ');

            Print(childDependency, builder);
        }
    }
}