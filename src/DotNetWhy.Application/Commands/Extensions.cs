namespace DotNetWhy.Application.Commands;

internal static class Extensions
{
    private static readonly string WorkingDirectory = Environment.CurrentDirectory;

    public static Request ToRequest(this DotNetWhyCommand.Settings settings) =>
        new(settings.PackageName, WorkingDirectory)
        {
            PackageVersion = settings.PackageVersion
        };
}