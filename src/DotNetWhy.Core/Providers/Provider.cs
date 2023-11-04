namespace DotNetWhy.Core.Providers;

internal sealed class Provider : IProvider
{
    public Response Get(Request request)
    {
        var name = Path.GetFileName(request.WorkingDirectory);

        try
        {
            return new RequestValidator(request)
                .HandleWith(new WorkingDirectoryValidator(request.WorkingDirectory))
                .HandleNext(new GetRestoreGraphOutputPathQuery())
                .Map(restoreGraphOutputPath =>
                    new GenerateRestoreGraphFileCommand(
                            request.WorkingDirectory,
                            restoreGraphOutputPath)
                        .HandleWith(new RestoreCommand(request.WorkingDirectory))
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