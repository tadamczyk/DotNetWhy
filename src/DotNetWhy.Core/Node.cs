namespace DotNetWhy.Core;

public sealed class Node(
    string name,
    string version,
    IEnumerable<Node> nodes)
{
    public string Name { get; } = name;
    public string Version { get; } = version;
    public IEnumerable<Node> Nodes { get; } = nodes;
    public bool HasNodes => Nodes.Any();
    public int LastNodesSum => HasNodes ? Nodes.Sum(node => node.LastNodesSum) : 1;

    public override string ToString() => $"{Name}{(Version is null ? string.Empty : $" ({Version})")}";
}