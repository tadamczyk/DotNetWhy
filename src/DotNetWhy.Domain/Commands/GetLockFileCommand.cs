namespace DotNetWhy.Domain.Commands;

internal record struct GetLockFileCommand(string WorkingDirectory) : ICommand;