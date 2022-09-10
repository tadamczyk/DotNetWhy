namespace DotNetWhy.Loggers.Services;

internal class DependencyTreeLogger : IDependencyTreeLogger
{
    private static string _packageName;
    private readonly IndexHelper _index = new();
    private readonly ILogger _logger;

    public DependencyTreeLogger(ILogger logger)
    {
        _logger = logger;
    }

    public void LogStartMessage(string workingDirectory)
    {
        _logger.LogLine($"Analyzing project(s) from {workingDirectory} directory...");
        _logger.LogLine();
    }

    public void LogResults(
        Solution solution,
        string packageName)
    {
        if (!solution.HasProjects)
        {
            _logger.LogLine($"Package {packageName} usage not found.");
            return;
        }

        _packageName = packageName;
        var labelWidth = ConsoleLoggerConstants.Widths.Label(solution);

        _logger.LogLine(
            GetLabel(
                ConsoleLoggerConstants.Prefixes.Solution,
                solution.Name,
                labelWidth,
                solution.DependencyCounter),
            Color.DarkCyan);

        solution.Projects.ForEach(project =>
        {
            _logger.LogLine(
                GetLabel(
                    ConsoleLoggerConstants.Prefixes.Project,
                    project.Name,
                    labelWidth,
                    solution.DependencyCounter,
                    project.DependencyCounter),
                Color.Green);

            project.Targets.ForEach(target =>
            {
                _logger.LogLine(
                    GetLabel(
                        ConsoleLoggerConstants.Prefixes.Target,
                        target.Name,
                        labelWidth,
                        project.DependencyCounter,
                        target.DependencyCounter),
                    Color.DarkGreen);

                _index.Reset();
                target.Dependencies.ForEach(dependency => LogDependencyTree(dependency));
            });

            _logger.LogLine();
        });
    }

    public void LogErrors(IEnumerable<string> errors)
    {
        errors.ForEach(error => _logger.LogLine(error, Color.Red));
    }

    public void LogEndMessage(TimeSpan elapsedTime)
    {
        _logger.LogLine($"Time elapsed: {elapsedTime:hh\\:mm\\:ss\\.ff}");
    }

    private static string GetLabel(
        string prefix,
        string name,
        int width,
        int all,
        int? count = null) =>
        $"{prefix} {$"{name}".PadRight(width)} {$"[{(count.HasValue ? $"{count}/" : string.Empty)}{all}]".PadLeft(ConsoleLoggerConstants.Widths.QuadrupleTab)}";

    private void LogDependencyTree(Dependency dependency, StringBuilder dependencyPathBuilder = null)
    {
        if (!dependency.HasDependencies)
        {
            LogDependencyPath(dependencyPathBuilder?.ToString());
            return;
        }

        foreach (var childDependency in dependency.Dependencies)
        {
            dependencyPathBuilder = (dependencyPathBuilder ?? new StringBuilder(dependency.ToString()))
                .Append(ConsoleLoggerConstants.Separators.Short).Append(childDependency);

            LogDependencyTree(childDependency, dependencyPathBuilder);
        }
    }

    private void LogDependencyPath(string dependencyPath)
    {
        _logger.Log($"{_index.Next()}.".PadRight(ConsoleLoggerConstants.Widths.DoubleTab));

        var dependencyPathParts = dependencyPath?.Split(ConsoleLoggerConstants.Separators.Short);
        var dependencyPathIndex = dependencyPathParts?.Length;
        var currentWidth = ConsoleLoggerConstants.Widths.DoubleTab;

        foreach (var dependencyPathPart in dependencyPathParts)
        {
            currentWidth += dependencyPathPart.Length + ConsoleLoggerConstants.Separators.Long.Length;
            if (currentWidth >= ConsoleLoggerConstants.Widths.Max)
            {
                currentWidth = ConsoleLoggerConstants.Widths.TripleTab;
                _logger.LogLine();
                _logger.Log(ConsoleLoggerConstants.Separators.Default, currentWidth);
            }

            _logger.Log(dependencyPathPart, dependencyPathPart.Contains(_packageName, StringComparison.InvariantCultureIgnoreCase) ? Color.Red : null);
            if (--dependencyPathIndex > 0) _logger.Log(ConsoleLoggerConstants.Separators.Long);
        }

        _logger.LogLine();
    }
}