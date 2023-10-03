namespace DotNetWhy.Common.Mediator;

internal sealed class Mediator(
        IServiceProvider serviceProvider)
    : IMediator
{
    public async Task SendAsync<TCommand>(
        TCommand command)
        where TCommand : ICommand =>
        await serviceProvider
            .GetRequiredService<ICommandHandler<TCommand>>()
            .HandleAsync(command);

    public async Task<TQueryResult> SendAsync<TQuery, TQueryResult>(
        TQuery query)
        where TQuery : IQuery =>
        await serviceProvider
            .GetRequiredService<IQueryHandler<TQuery, TQueryResult>>()
            .QueryAsync(query);
}