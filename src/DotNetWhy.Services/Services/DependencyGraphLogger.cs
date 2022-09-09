namespace DotNetWhy.Services.Services;

internal class DependencyGraphLogger : IDependencyGraphLogger
{
    private static int _iterator;
    private static string _packageName;
    private static Separator _separator;
    private static Width _width;

    private record Separator(char Short, string Long);
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

        _packageName = packageName;
        _separator = new Separator(':', " -> ");
        _width = new Width(
            (_logger.Configuration.MaxWidth >= 144 ? 144 : _logger.Configuration.MaxWidth >= 120 ? 120 : 80) - Widths.DoubleTab,
            GetLabelWidth(solution));

        _logger.LogLine(
            GetLabel(
                Prefixes.Solution,
                solution.Name,
                _width.Label,
                solution.DependencyCounter),
            Color.DarkCyan);

        foreach (var project in solution.Projects)
        {
            _logger.LogLine(
                GetLabel(
                    Prefixes.Project,
                    project.Name,
                    _width.Label,
                    solution.DependencyCounter,
                    project.DependencyCounter),
                Color.Green);

            foreach (var target in project.Targets)
            {
                _logger.LogLine(
                    GetLabel(
                        Prefixes.Target,
                        target.Name,
                        _width.Label,
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

    private void Print(Dependency dependency, StringBuilder dependencyPathBuilder = null)
    {
        if (!dependency.HasDependencies)
        {
            _logger.Log($"{++_iterator}.".PadRight(Widths.DoubleTab));

            var dependencyPath = dependencyPathBuilder?.ToString();
            var dependencyPathParts = dependencyPath?.Split(_separator.Short);
            var dependencyPathIndex = dependencyPathParts?.Length;
            var currentWidth = Widths.DoubleTab;

            foreach (var dependencyPathPart in dependencyPathParts)
            {
                currentWidth += dependencyPathPart.Length + _separator.Long.Length;
                if (currentWidth >= _width.Max)
                {
                    currentWidth = Widths.TripleTab;
                    _logger.LogLine();
                    _logger.Log(' ', currentWidth);
                }

                _logger.Log(dependencyPathPart, dependencyPathPart.Contains(_packageName) ? Color.Red : null);
                if (--dependencyPathIndex > 0) _logger.Log(_separator.Long);
            }

            _logger.LogLine();

            return;
        }

        foreach (var childDependency in dependency.Dependencies)
        {
            dependencyPathBuilder = (dependencyPathBuilder ?? new StringBuilder(dependency.ToString()))
                .Append(_separator.Short)
                .Append(childDependency);

            Print(childDependency, dependencyPathBuilder);
        }
    }
}