namespace DotNetWhy.Domain;

public readonly record struct DependencyTreeNode
{
    private const StringComparison StringComparisonType = StringComparison.OrdinalIgnoreCase;

    private readonly List<DependencyTreeNode> _nodes = new();

    private bool IsMatchingNode(
        string name,
        string version = null) =>
        Name.ToLower().Contains(name.ToLower())
        && (version is null || Version.Equals(version, StringComparisonType));

    private DependencyTreeNode(
        string name,
        string version = null)
    {
        Name = name;
        Version = version;
    }

    public DependencyTreeNode() =>
        throw new InitializeDependencyTreeNodeFailedException();

    public static DependencyTreeNode Create(
        string name,
        string version = null) =>
        new(name, version);

    public string Name { get; }

    public string Version { get; }

    public IEnumerable<DependencyTreeNode> Nodes =>
        _nodes.OrderBy(node => node.Name);

    public bool HasNodes =>
        _nodes.Any();

    public int NodesCount =>
        HasNodes
            ? _nodes.Sum(node => node.NodesCount)
            : 1;

    public void AddNode(DependencyTreeNode node) =>
        _nodes.Add(node);

    public bool ContainsNode(
        string name,
        string version = null) =>
        HasNodes
        || IsMatchingNode(name, version);

    public override string ToString() =>
        $"{Name}{(Version is null ? string.Empty : $" ({Version})")}";
}