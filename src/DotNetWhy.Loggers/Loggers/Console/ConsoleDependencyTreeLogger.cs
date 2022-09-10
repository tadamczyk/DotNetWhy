﻿namespace DotNetWhy.Loggers.Console;

internal class ConsoleDependencyTreeLogger : BaseDependencyTreeLogger, IDependencyTreeLogger
{
    private readonly IndexHelper _index = new();
    private readonly ILogger _logger;

    public ConsoleDependencyTreeLogger(ILogger logger)
        : base(logger)
    {
        _logger = logger;
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

        packageName.SetAsSearchedPackageName();
        solution.SetLabelWidth();

        _logger.LogLine(solution.GetSolutionLabel(), Color.DarkCyan);

        solution.Projects.ForEach(project =>
        {
            _logger.LogLine(project.GetProjectLabel(solution.DependencyCounter), Color.Green);

            project.Targets.ForEach(target =>
            {
                _index.Reset();
                _logger.LogLine(target.GetTargetLabel(project.DependencyCounter), Color.DarkGreen);

                target.Dependencies.ForEach(dependency => LogDependencyTree(dependency));
            });

            _logger.LogLine();
        });
    }

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
                .Append(ConsoleLoggerConstants.Separators.Short)
                .Append(childDependency);

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

            _logger.Log(dependencyPathPart, dependencyPathPart.Contains(ConsoleLoggerExtensions.SearchedPackageName, StringComparison.InvariantCultureIgnoreCase) ? Color.Red : null);
            if (--dependencyPathIndex > 0) _logger.Log(ConsoleLoggerConstants.Separators.Long);
        }

        _logger.LogLine();
    }
}