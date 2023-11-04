namespace DotNetWhy.Application.Commands;

[Description("Shows information about why a NuGet package is installed")]
internal sealed partial class DotNetWhyCommand
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<NAME>")]
        [Description("The NuGet package name")]
        public string PackageName { get; init; }

        [CommandOption("-v|--version <VERSION>")]
        [Description("The NuGet package version")]
        public string PackageVersion { get; init; }
    }
}