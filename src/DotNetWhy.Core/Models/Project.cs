namespace DotNetWhy.Core.Models;

public record Project(string Name)
{
    private readonly IList<Target> _targets = new List<Target>();

    public ImmutableArray<Target> Targets => _targets.ToImmutableArray();

    public int DependencyCounter => _targets.Sum(target => target.DependencyCounter);

    public bool HasTargets => _targets.Any();

    internal void AddTarget(Target target) => _targets.Add(target);
}