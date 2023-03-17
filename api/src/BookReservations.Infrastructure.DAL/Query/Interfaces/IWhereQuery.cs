using System.Linq.Expressions;

namespace BookReservations.Infrastructure.DAL.Query.Interfaces;

public interface IWhereQuery<TEntity>
    where TEntity : class, IBaseEntity
{
    IAfterWhereQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
}