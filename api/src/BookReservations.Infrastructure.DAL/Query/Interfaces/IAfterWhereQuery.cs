namespace BookReservations.Infrastructure.DAL.Query.Interfaces;

public interface IAfterWhereQuery<TEntity> :
    IQueryExecution<TEntity>,
    IOrderByQuery<TEntity>,
    IPageQuery<TEntity>,
    IAndOrWhereQuery<TEntity>
    where TEntity : class, IBaseEntity
{
}
