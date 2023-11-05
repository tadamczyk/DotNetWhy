namespace DotNetWhy.Core.Entities;

internal record struct DependencyNode
(
    string Name,
    string Version = null
)
{
    private const StringComparison StringComparisonType = StringComparison.OrdinalIgnoreCase;

    private readonly List<DependencyNode> _nodes = new();

    private bool ContainsNode(DependencyNode node) =>
        _nodes.Count != 0
        || IsMatchingNode(node);

    private bool IsMatchingNode(DependencyNode node) =>
        Name.Contains(node.Name.ToLower(), StringComparisonType)
        && (node.Version is null || Version.Equals(node.Version, StringComparisonType));

    internal void AddMatchingNodes(
        IEnumerable<DependencyNode> nodes,
        DependencyNode searchNode) =>
        _nodes.AddRange(nodes.Where(node => node.ContainsNode(searchNode)));

    internal IEnumerable<DependencyNode> GetNodes() =>
        _nodes.OrderBy(node => node.Name);
}