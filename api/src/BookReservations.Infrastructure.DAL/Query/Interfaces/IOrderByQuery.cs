using System.Linq.Expressions;

namespace BookReservations.Infrastructure.DAL.Query.Interfaces;

public interface IOrderByQuery<TEntity>
    where TEntity : class, IBaseEntity

{
    IPageQuery<TEntity> OrderBy(Expression<Func<TEntity, object>> keySelector, bool ascendingOrder = true);
    IPageQuery<TEntity> OrderBy(string propertyName, bool ascendingOrder = true);
}