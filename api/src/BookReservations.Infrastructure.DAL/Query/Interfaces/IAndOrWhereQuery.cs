using System.Linq.Expressions;

namespace BookReservations.Infrastructure.DAL.Query.Interfaces;

public interface IAndOrWhereQuery<TEntity> : IQueryExecution<TEntity>
    where TEntity : class, IBaseEntity
{
    IAfterWhereQuery<TEntity> AndWhere(Expression<Func<TEntity, bool>> predicate);
    IAfterWhereQuery<TEntity> OrWhere(Expression<Func<TEntity, bool>> predicate);
}
