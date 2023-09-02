namespace DotNetWhy.Common;

public interface IMediator
{
    Task SendAsync<TCommand>(
        TCommand command)
        where TCommand : ICommand;

    Task<TQueryResult> SendAsync<TQuery, TQueryResult>(
        TQuery query)
        where TQuery : IQuery;
}