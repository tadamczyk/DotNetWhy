namespace DotNetWhy.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services) =>
        services
            .AddCli()
            .AddMediator()
            .AddSingleton<ICommandHandler<GenerateRestoreGraphFileCommand>,
                GenerateRestoreGraphFileCommandHandler>()
            .AddSingleton<ICommandHandler<RestoreProjectCommand>,
                RestoreProjectCommandHandler>()
            .AddSingleton<IQueryHandler<GetDependencyTreeQuery, DependencyTreeNode>,
                GetDependencyTreeQueryHandler>()
            .AddSingleton<IDependencyGraphSpecProvider, DependencyGraphSpecProvider>()
            .AddSingleton<IDependencyTreeProvider, DependencyTreeProvider>()
            .AddSingleton<ILockFileProvider, LockFileProvider>()
            .AddSingleton<ILockFileTargetLibraryProvider, LockFileTargetLibraryProvider>()
            .AddSingleton<ILockFileTargetProvider, LockFileTargetProvider>();
}