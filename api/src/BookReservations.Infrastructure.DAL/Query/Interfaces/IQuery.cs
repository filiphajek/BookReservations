namespace BookReservations.Infrastructure.DAL.Query.Interfaces;

public interface IQuery<TEntity> :
    IWhereQuery<TEntity>,
    IOrderByQuery<TEntity>,
    IPageQuery<TEntity>,
    IQueryExecution<TEntity>,
    IJoinQuery<TEntity>
    where TEntity : class, IBaseEntity
{
}