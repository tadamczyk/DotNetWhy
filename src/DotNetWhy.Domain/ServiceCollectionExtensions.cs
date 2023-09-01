namespace DotNetWhy.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services) =>
        services
            .AddMediator()
            .AddSingleton<ICommandHandler<ConvertDependencyGraphSpecCommand, Node>,
                ConvertDependencyGraphSpecCommandHandler>()
            .AddSingleton<ICommandHandler<CreateLockFilesCommand>,
                CreateLockFilesCommandHandler>()
            .AddSingleton<ICommandHandler<GetDependencyGraphSpecCommand, DependencyGraphSpec>,
                GetDependencyGraphSpecCommandHandler>()
            .AddSingleton<ICommandHandler<GetLockFileCommand, LockFile>,
                GetLockFileCommandHandler>()
            .AddSingleton<IDependencyGraphProvider, DependencyGraphProvider>();
}