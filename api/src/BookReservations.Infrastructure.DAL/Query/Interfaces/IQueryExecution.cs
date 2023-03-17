namespace BookReservations.Infrastructure.DAL.Query.Interfaces;

public interface IQueryExecution<TEntity>
    where TEntity : class, IBaseEntity
{
    Task<QueryResult<TEntity>> ExecuteAsync(CancellationToken cancellationToken = default);
}
