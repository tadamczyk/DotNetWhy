namespace DotNetWhy.Domain.Commands;

internal record struct GetDependencyGraphSpecCommand(string WorkingDirectory) : ICommand;