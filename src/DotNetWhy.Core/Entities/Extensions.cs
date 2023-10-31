namespace DotNetWhy.Core.Entities;

internal static class Extensions
{
    public static Node ToNode(this DependencyNode dependencyNode) =>
        new(
            dependencyNode.Name,
            dependencyNode.Version,
            dependencyNode.GetNodes().Select(ToNode));
}