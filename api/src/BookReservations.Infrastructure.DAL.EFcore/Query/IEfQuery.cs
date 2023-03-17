using BookReservations.Infrastructure.DAL.Query.Interfaces;

namespace BookReservations.Infrastructure.DAL.EFcore.Query;

public interface IEfQuery<TEntity> : IQuery<TEntity>, IAfterWhereQuery<TEntity>
    where TEntity : class, IBaseEntity
{
}
