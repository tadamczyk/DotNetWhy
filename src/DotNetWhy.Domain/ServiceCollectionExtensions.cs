namespace DotNetWhy.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services) =>
        services
            .AddSingleton<ICommandHandler<ConvertDependencyGraphSpecCommand, Solution>,
                ConvertDependencyGraphSpecCommandHandler>()
            .AddSingleton<ICommandHandler<CreateLockFilesCommand>,
                CreateLockFilesCommandHandler>()
            .AddSingleton<ICommandHandler<GetDependencyGraphSpecCommand, DependencyGraphSpec>,
                GetDependencyGraphSpecCommandHandler>()
            .AddSingleton<ICommandHandler<GetLockFileCommand, LockFile>,
                GetLockFileCommandHandler>()
            .AddSingleton<IMediator, Mediator.Mediator>()
            .AddSingleton<IDependencyGraphProvider, DependencyGraphProvider>();
}