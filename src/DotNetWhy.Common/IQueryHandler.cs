namespace DotNetWhy.Common;

public interface IQueryHandler<in TQuery, TQueryResult>
    where TQuery : IQuery
{
    Task<TQueryResult> QueryAsync(TQuery query);
}