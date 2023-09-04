namespace DotNetWhy.Common.Mediator;

internal sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    public async Task SendAsync<TCommand>(
        TCommand command)
        where TCommand : ICommand =>
        await _serviceProvider
            .GetRequiredService<ICommandHandler<TCommand>>()
            .HandleAsync(command);

    public async Task<TQueryResult> SendAsync<TQuery, TQueryResult>(
        TQuery query)
        where TQuery : IQuery =>
        await _serviceProvider
            .GetRequiredService<IQueryHandler<TQuery, TQueryResult>>()
            .QueryAsync(query);
}