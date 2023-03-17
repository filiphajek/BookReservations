namespace BookReservations.Infrastructure.DAL.Query.Interfaces;

public interface IAfterJoinQuery<TEntity> : IWhereQuery<TEntity>,
    IOrderByQuery<TEntity>,
    IPageQuery<TEntity>,
    IQueryExecution<TEntity>
    where TEntity : class, IBaseEntity
{
}
