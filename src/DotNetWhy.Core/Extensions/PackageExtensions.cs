namespace DotNetWhy.Core.Extensions;

internal static class PackageExtensions
{
    internal static bool IsPackageReference(this PackageSpec package) =>
        package.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference;
}