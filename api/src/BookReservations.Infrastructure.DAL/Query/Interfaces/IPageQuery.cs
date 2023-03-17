namespace BookReservations.Infrastructure.DAL.Query.Interfaces;

public interface IPageQuery<TEntity> : IQueryExecution<TEntity>
    where TEntity : class, IBaseEntity
{
    IQueryExecution<TEntity> Page(int page, int pageSize = 10, bool includeTotalCount = false);
}