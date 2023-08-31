namespace DotNetWhy.Domain.CommandHandlers;

internal sealed class ConvertDependencyGraphSpecCommandHandler
    : ICommandHandler<ConvertDependencyGraphSpecCommand, Solution>
{
    private readonly IMediator _mediator;

    private string PackageName { get; set; }
    private string PackageVersion { get; set; }

    public ConvertDependencyGraphSpecCommandHandler(IMediator mediator) =>
        _mediator = mediator;

    public Solution Handle(ConvertDependencyGraphSpecCommand command)
    {
        PackageName = command.PackageName;
        PackageVersion = command.PackageVersion;
        var solution = new Solution(command.SolutionName);
        var sourceProjects = GetSourceProjects(command.DependencyGraphSpec);

        sourceProjects.ForEach(sourceProject =>
        {
            var project = new Project(sourceProject.Name);
            var sourceProjectLockFile = GetSourceProjectLockFile(sourceProject.RestoreMetadata.OutputPath);
            if (sourceProjectLockFile is null) return;

            sourceProject.TargetFrameworks.ForEach(sourceTarget =>
            {
                var target = new Target(sourceTarget.FrameworkName.ToString());
                var sourceTargetLockFile =
                    GetSourceTargetLockFile(sourceProjectLockFile, sourceTarget.FrameworkName.ToString());
                if (sourceTargetLockFile is null) return;

                sourceTarget.Dependencies.ForEach(sourceDependency =>
                {
                    var sourceLibraryLockFile = GetSourceLibraryLockFile(sourceTargetLockFile, sourceDependency.Name);
                    if (sourceLibraryLockFile is null) return;

                    var dependency = sourceLibraryLockFile.ToDependency();
                    CreateDependenciesPaths(sourceTargetLockFile, sourceLibraryLockFile, dependency);

                    if (dependency.HasDependencies || dependency.IsOrContainsPackage(PackageName, PackageVersion))
                        target.AddDependency(dependency);
                });
                if (target.HasDependencies) project.AddTarget(target);
            });
            if (project.HasTargets) solution.AddProject(project);
        });

        return solution;
    }

    private IEnumerable<PackageSpec> GetSourceProjects(
        DependencyGraphSpec dependencyGraphSpec) =>
        dependencyGraphSpec.Projects.Where(p => p.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference);

    private LockFile GetSourceProjectLockFile(string outputPath) =>
        _mediator.Send<GetLockFileCommand, LockFile>(new GetLockFileCommand(outputPath));

    private LockFileTarget GetSourceTargetLockFile(
        LockFile sourceProjectLockFile,
        string name) =>
        sourceProjectLockFile.Targets.FirstOrDefault(t => t.TargetFramework.ToString().Equals(name));

    private LockFileTargetLibrary GetSourceLibraryLockFile(
        LockFileTarget sourceTargetLockFile,
        string name) =>
        sourceTargetLockFile.Libraries.FirstOrDefault(l => l.Name.Equals(name));

    private void CreateDependenciesPaths(
        LockFileTarget sourceTargetLockFile,
        LockFileTargetLibrary sourceLibraryLockFile,
        Dependency dependency)
    {
        if (!sourceLibraryLockFile.Dependencies.Any()) return;

        sourceLibraryLockFile.Dependencies.ForEach(libraryDependency =>
        {
            var childSourceLibraryLockFile = GetSourceLibraryLockFile(sourceTargetLockFile, libraryDependency.Id);
            if (childSourceLibraryLockFile is null) return;

            var childDependency = childSourceLibraryLockFile.ToDependency();
            CreateDependenciesPaths(sourceTargetLockFile, childSourceLibraryLockFile, childDependency);

            if (childDependency.IsOrContainsPackage(PackageName, PackageVersion))
                dependency.AddDependency(childDependency);
        });
    }
}