namespace DotNetWhy.Core.Extensions;

internal static class PackageExtensions
{
    public static bool IsPackageReference(this PackageSpec package) =>
        package.RestoreMetadata.ProjectStyle == ProjectStyle.PackageReference;
}