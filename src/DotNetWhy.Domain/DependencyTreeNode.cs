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

    internal void AddNodes(IEnumerable<DependencyTreeNode> nodes) =>
        _nodes.AddRange(nodes);

    internal bool ContainsNode(
        string name,
        string version = null) =>
        HasNodes
        || IsMatchingNode(name, version);

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

    public int LastNodesSum =>
        HasNodes
            ? _nodes.Sum(node => node.LastNodesSum)
            : 1;

    public override string ToString() =>
        $"{Name}{(Version is null ? string.Empty : $" ({Version})")}";
}