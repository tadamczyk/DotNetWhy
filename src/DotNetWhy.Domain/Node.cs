namespace DotNetWhy.Domain;

public readonly record struct Node(
    string Name,
    string Version = null)
{
    private const StringComparison StringComparisonType = StringComparison.OrdinalIgnoreCase;

    private readonly List<Node> _nodes = new();

    private bool IsMatchingNode(
        string name,
        string version = null) =>
        Name.Contains(name, StringComparisonType)
        && (version is null || Version.Equals(version, StringComparisonType));

    public IEnumerable<Node> Nodes =>
        _nodes.OrderBy(node => node.Name);

    public bool HasNodes =>
        _nodes.Any();

    public int NodesCount =>
        HasNodes
            ? _nodes.Sum(node => node.NodesCount)
            : 1;

    public void AddNode(Node node) =>
        _nodes.Add(node);

    public bool ContainsNode(
        string name,
        string version = null) =>
        HasNodes
        || IsMatchingNode(name, version);

    public override string ToString() =>
        $"{Name}{(Version is null ? string.Empty : $" ({Version})")}";
}