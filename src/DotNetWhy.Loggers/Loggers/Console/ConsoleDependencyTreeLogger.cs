namespace DotNetWhy.Loggers.Console;

internal class ConsoleDependencyTreeLogger : BaseDependencyTreeLogger, IDependencyTreeLogger
{
    private readonly IndexHelper _index = new();
    private readonly ILogger _logger;

    public ConsoleDependencyTreeLogger(ILogger logger)
        : base(logger) =>
        _logger = logger;

    public void LogResults(
        Node solution,
        string packageName,
        string packageVersion)
    {
        if (!solution.HasNodes)
        {
            _logger.LogLine($"Package {packageName}{(string.IsNullOrEmpty(packageVersion) ? "" : $" {packageVersion}")} usage not found.");
            return;
        }

        packageName.SetAsSearchedPackageName();
        solution.SetLabelWidth();

        _logger.LogLine(solution.GetSolutionLabel(), Color.DarkCyan);

        foreach (var project in solution.Nodes)
        {
            _logger.LogLine(project.GetProjectLabel(solution.LastNodesSum), Color.Green);

            foreach (var target in project.Nodes)
            {
                _index.Reset();
                _logger.LogLine(target.GetTargetLabel(project.LastNodesSum), Color.DarkGreen);

                foreach (var dependency in target.Nodes)
                {
                    LogDependencyTree(dependency);
                }
            }

            _logger.LogLine();
        }
    }

    private void LogDependencyTree(Node dependency, StringBuilder dependencyPathBuilder = null)
    {
        if (!dependency.HasNodes)
        {
            LogDependencyPath(dependencyPathBuilder?.ToString() ?? dependency.ToString());
            return;
        }

        foreach (var childDependency in dependency.Nodes)
        {
            var currentDependencyPathLength = dependencyPathBuilder?.Length ?? dependency.ToString().Length;

            dependencyPathBuilder = (dependencyPathBuilder ?? new StringBuilder(dependency.ToString()))
                .Append(ConsoleLoggerConstants.Separators.Short)
                .Append(childDependency);

            LogDependencyTree(childDependency, dependencyPathBuilder);

            dependencyPathBuilder.Remove(
                currentDependencyPathLength,
                dependencyPathBuilder.Length - currentDependencyPathLength);
        }
    }

    private void LogDependencyPath(string dependencyPath)
    {
        _logger.Log($"{_index.Next()}.".PadRight(ConsoleLoggerConstants.Widths.DoubleTab));

        var dependencyPathParts = dependencyPath?.Split(ConsoleLoggerConstants.Separators.Short);
        var dependencyPathIndex = dependencyPathParts?.Length;
        var currentWidth = ConsoleLoggerConstants.Widths.DoubleTab;

        foreach (var dependencyPathPart in dependencyPathParts!)
        {
            var dependencyPathPartWidth = dependencyPathPart.Length + ConsoleLoggerConstants.Separators.Long.Length;
            currentWidth += dependencyPathPartWidth;
            if (currentWidth >= ConsoleLoggerConstants.Widths.Max)
            {
                currentWidth = ConsoleLoggerConstants.Widths.TripleTab + dependencyPathPartWidth;
                _logger.LogLine();
                _logger.Log(ConsoleLoggerConstants.Separators.Default, ConsoleLoggerConstants.Widths.TripleTab);
            }

            _logger.Log(dependencyPathPart,
                dependencyPathPart.Contains(ConsoleLoggerExtensions.SearchedPackageName,
                    StringComparison.InvariantCultureIgnoreCase)
                    ? Color.Red
                    : null);
            if (--dependencyPathIndex > 0) _logger.Log(ConsoleLoggerConstants.Separators.Long);
        }

        _logger.LogLine();
    }
}