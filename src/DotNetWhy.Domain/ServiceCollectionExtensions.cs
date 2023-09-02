namespace DotNetWhy.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services) =>
        services
            .AddMediator()
            .AddSingleton<ICommandHandler<GenerateRestoreGraphFileCommand>,
                GenerateRestoreGraphFileCommandHandler>()
            .AddSingleton<ICommandHandler<RestoreProjectCommand>,
                RestoreProjectCommandHandler>()
            .AddSingleton<IQueryHandler<GetDependencyTreeNodeQuery, DependencyTreeNode>,
                GetDependencyTreeNodeQueryHandler>()
            .AddSingleton<IDependencyTreeProvider, DependencyTreeProvider>();
}