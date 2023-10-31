namespace DotNetWhy.Core.Providers;

internal sealed class Provider : IProvider
{
    public Response Get(Request request)
    {
        var workingDirectory = Environment.CurrentDirectory;
        var name = Path.GetFileName(workingDirectory);

        try
        {
            return new RequestValidator(request)
                .HandleWith(new WorkingDirectoryValidator(workingDirectory))
                .HandleNext(new GetRestoreGraphOutputPathQuery())
                .Map(restoreGraphOutputPath =>
                    new GenerateRestoreGraphFileCommand(
                            workingDirectory,
                            restoreGraphOutputPath)
                        .HandleWith(new RestoreCommand(workingDirectory))
                        .HandleNext(new GetDependencyNodeQuery(
                            restoreGraphOutputPath,
                            name,
                            request.PackageName,
                            request.PackageVersion)))
                .Unwrap()
                .Map(dependencyNode => dependencyNode.ToNode())
                .ToResponse();
        }
        catch (Exception exception)
        {
            return Result<Node>
                .Failure(exception.Message)
                .ToResponse();
        }
    }
}