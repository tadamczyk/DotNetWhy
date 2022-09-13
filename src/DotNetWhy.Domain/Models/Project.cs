namespace DotNetWhy.Domain;

public record Project(string Name)
{
    private readonly List<Target> _targets = new();

    public IReadOnlyCollection<Target> Targets => _targets.AsReadOnly();

    public int DependencyPathCounter => _targets.Sum(target => target.DependencyPathCounter);

    public bool HasTargets => _targets.Any();

    public void AddTarget(Target target) => _targets.Add(target);
}