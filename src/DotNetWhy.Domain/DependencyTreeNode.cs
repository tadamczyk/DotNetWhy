namespace DotNetWhy.Domain;

public record struct DependencyTreeNode(
    string Name,
    string Version = null)
{
    private const StringComparison StringComparisonType = StringComparison.OrdinalIgnoreCase;

    private readonly List<DependencyTreeNode> _nodes = new();

    private bool ContainsNode(
        DependencyTreeNode node) =>
        HasNodes
        || IsMatchingNode(node);

    private bool IsMatchingNode(
        DependencyTreeNode node) =>
        Name.ToLower().Contains(node.Name.ToLower())
        && (node.Version is null || Version.Equals(node.Version, StringComparisonType));

    internal void AddMatchingNodes(
        IEnumerable<DependencyTreeNode> nodes,
        DependencyTreeNode searchNode) =>
        _nodes.AddRange(nodes.Where(node => node.ContainsNode(searchNode)));

    public IEnumerable<DependencyTreeNode> Nodes =>
        _nodes.OrderBy(node => node.Name).AsEnumerable();

    public bool HasNodes =>
        _nodes.Any();

    public int LastNodesSum =>
        HasNodes
            ? _nodes.Sum(node => node.LastNodesSum)
            : 1;

    public override string ToString() =>
        $"{Name}{(Version is null ? string.Empty : $" ({Version})")}";
}