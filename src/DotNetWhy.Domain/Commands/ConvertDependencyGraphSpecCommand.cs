namespace DotNetWhy.Domain.Commands;

internal record struct ConvertDependencyGraphSpecCommand(
        DependencyGraphSpec DependencyGraphSpec,
        string SolutionName,
        string PackageName,
        string PackageVersion)
    : ICommand;

internal sealed class ConvertDependencyGraphSpecCommandHandler
    : ICommandHandler<ConvertDependencyGraphSpecCommand, Node>
{
    private readonly IMediator _mediator;

    private string PackageName { get; set; }
    private string PackageVersion { get; set; }

    public ConvertDependencyGraphSpecCommandHandler(IMediator mediator) =>
        _mediator = mediator;

    public Node Handle(ConvertDependencyGraphSpecCommand command)
    {
        PackageName = command.PackageName;
        PackageVersion = command.PackageVersion;

        var solution = new Node(command.SolutionName);

        var packageSpecs = GetPackageSpecs(command.DependencyGraphSpec);

        packageSpecs.ForEach(packageSpec =>
        {
            var project = new Node(packageSpec.Name);

            var lockFile = GetLockFile(packageSpec.RestoreMetadata.OutputPath);
            if (lockFile is null) return;

            packageSpec.TargetFrameworks.ForEach(targetFramework =>
            {
                var target = new Node(targetFramework.FrameworkName.ToString());

                var lockFileTarget = GetLockFileTargetByName(lockFile, targetFramework.FrameworkName.ToString());
                if (lockFileTarget is null) return;

                targetFramework.Dependencies.ForEach(dependency =>
                {
                    var lockFileTargetLibrary = GetLockFileTargetLibraryByName(lockFileTarget, dependency.Name);
                    if (lockFileTargetLibrary is null) return;

                    var library = new Node(lockFileTargetLibrary.Name, lockFileTargetLibrary.Version.ToString());
                    CreatePaths(lockFileTarget, lockFileTargetLibrary, library);

                    if (library.ContainsNode(PackageName, PackageVersion)) target.AddNode(library);
                });

                if (target.HasNodes) project.AddNode(target);
            });

            if (project.HasNodes) solution.AddNode(project);
        });

        return solution;
    }

    private static IEnumerable<PackageSpec> GetPackageSpecs(DependencyGraphSpec dependencyGraphSpec) =>
        dependencyGraphSpec
            .Projects
            .Where(packageSpec => packageSpec.RestoreMetadata.ProjectStyle is ProjectStyle.PackageReference);

    private LockFile GetLockFile(string workingDirectory) =>
        _mediator.Send<GetLockFileCommand, LockFile>(new GetLockFileCommand(workingDirectory));

    private static LockFileTarget GetLockFileTargetByName(
        LockFile lockFile,
        string targetFrameworkName) =>
        lockFile
            .Targets
            .FirstOrDefault(target => target.TargetFramework.ToString().Equals(targetFrameworkName));

    private static LockFileTargetLibrary GetLockFileTargetLibraryByName(
        LockFileTarget lockFileTarget,
        string libraryName) =>
        lockFileTarget
            .Libraries
            .FirstOrDefault(library => library.Name.Equals(libraryName));

    private void CreatePaths(
        LockFileTarget lockFileTarget,
        LockFileTargetLibrary lockFileTargetLibrary,
        Node library)
    {
        if (lockFileTargetLibrary.Dependencies.IsNullOrEmpty()) return;

        lockFileTargetLibrary.Dependencies.ForEach(dependency =>
        {
            var childLockFileTargetLibrary = GetLockFileTargetLibraryByName(lockFileTarget, dependency.Id);
            if (childLockFileTargetLibrary is null) return;

            var childLibrary = new Node(childLockFileTargetLibrary.Name, childLockFileTargetLibrary.Version.ToString());
            CreatePaths(lockFileTarget, childLockFileTargetLibrary, childLibrary);

            if (childLibrary.ContainsNode(PackageName, PackageVersion)) library.AddNode(childLibrary);
        });
    }
}