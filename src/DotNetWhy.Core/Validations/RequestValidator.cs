namespace DotNetWhy.Core.Validations;

internal sealed class RequestValidator
(
    Request request
) : IResultHandler
{
    public Result Handle() =>
        !string.IsNullOrEmpty(request.PackageName)
            ? Result.Success()
            : Result.Failure(Errors.PackageNameNotSpecified);

    private static class Errors
    {
        public const string PackageNameNotSpecified =
            "Package name not specified. Please run command once again specifying package name - 'dotnet why PACKAGE_NAME'.";
    }
}