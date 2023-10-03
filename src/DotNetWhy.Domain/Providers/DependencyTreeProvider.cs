namespace DotNetWhy.Domain.Providers;

internal sealed class DependencyTreeProvider(
        IMediator mediator)
    : IDependencyTreeProvider
{
    public async Task<DependencyTreeNode> GetAsync(DependencyTreeParameters parameters)
    {
        var name = GetName(parameters.WorkingDirectory);
        var restoreGraphOutputPath = GetRestoreGraphOutputPath();

        await Task.WhenAll(
            RestoreProjects(parameters.WorkingDirectory),
            GenerateRestoreGraphFile(parameters.WorkingDirectory, restoreGraphOutputPath));

        var dependencyTree = await GetDependencyTree(
            restoreGraphOutputPath,
            name,
            parameters.PackageName,
            parameters.PackageVersion);

        return dependencyTree;
    }

    private static string GetName(string workingDirectory) =>
        Path.GetFileName(workingDirectory) ?? workingDirectory;

    private static string GetRestoreGraphOutputPath() =>
        Path.Combine(
            Path.GetTempPath(),
            Path.GetTempFileName().Replace(FileExtensions.Temp, FileExtensions.Json));

    private async Task RestoreProjects(string workingDirectory) =>
        await mediator.SendAsync(
            new RestoreProjectCommand(workingDirectory));

    private async Task GenerateRestoreGraphFile(
        string workingDirectory,
        string restoreGraphOutputPath) =>
        await mediator.SendAsync(
            new GenerateRestoreGraphFileCommand(
                workingDirectory,
                restoreGraphOutputPath));

    private async Task<DependencyTreeNode> GetDependencyTree(
        string restoreGraphOutputPath,
        string name,
        string packageName,
        string packageVersion = null) =>
        await mediator.SendAsync<GetDependencyTreeQuery, DependencyTreeNode>(
            new GetDependencyTreeQuery(
                restoreGraphOutputPath,
                name,
                packageName,
                packageVersion));
}