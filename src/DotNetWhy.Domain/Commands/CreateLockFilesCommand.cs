namespace DotNetWhy.Domain.Commands;

internal record struct CreateLockFilesCommand(string WorkingDirectory) : ICommand;