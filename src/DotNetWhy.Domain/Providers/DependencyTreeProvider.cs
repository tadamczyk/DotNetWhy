namespace DotNetWhy.Domain.Providers;

internal sealed class DependencyTreeProvider : IDependencyTreeProvider
{
    private readonly IMediator _mediator;

    public DependencyTreeProvider(IMediator mediator) =>
        _mediator = mediator;

    public async Task<DependencyTreeNode> GetAsync(
        string workingDirectory,
        string packageName,
        string packageVersion = null)
    {
        var name = GetName(workingDirectory);
        var restoreGraphOutputPath = GetRestoreGraphOutputPath();

        await Task.WhenAll(
            RestoreProjects(workingDirectory),
            GenerateRestoreGraphFile(workingDirectory, restoreGraphOutputPath));

        var dependencyTreeNode = await GetDependencyTreeNode(
            restoreGraphOutputPath,
            name,
            packageName,
            packageVersion);

        return dependencyTreeNode;
    }

    private static string GetName(string workingDirectory) =>
        Path.GetFileName(workingDirectory) ?? workingDirectory;

    private static string GetRestoreGraphOutputPath() =>
        Path.Combine(
            Path.GetTempPath(),
            Path.GetTempFileName().Replace(FileExtensions.Temp, FileExtensions.Json));

    private async Task RestoreProjects(string workingDirectory) =>
        await _mediator.SendAsync(
            new RestoreProjectCommand(workingDirectory));

    private async Task GenerateRestoreGraphFile(
        string workingDirectory,
        string restoreGraphOutputPath) =>
        await _mediator.SendAsync(
            new GenerateRestoreGraphFileCommand(
                workingDirectory,
                restoreGraphOutputPath));

    private async Task<DependencyTreeNode> GetDependencyTreeNode(
        string restoreGraphOutputPath,
        string name,
        string packageName,
        string packageVersion = null) =>
        await _mediator.SendAsync<GetDependencyTreeQuery, DependencyTreeNode>(
            new GetDependencyTreeQuery(
                restoreGraphOutputPath,
                name,
                packageName,
                packageVersion));
}