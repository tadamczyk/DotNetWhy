namespace DotNetWhy.Core.Queries;

internal sealed class GetRestoreGraphOutputPathQuery : IResultHandler<string>
{
    private const string TempFileExtension = ".tmp";
    private const string JsonFileExtension = ".json";

    public Result<string> Handle()
    {
        var tempPath = Path.GetTempPath();
        var tempFile = Path.GetTempFileName().Replace(TempFileExtension, JsonFileExtension);
        var path = Path.Combine(tempPath, tempFile);

        return Result<string>.Success(path);
    }
}