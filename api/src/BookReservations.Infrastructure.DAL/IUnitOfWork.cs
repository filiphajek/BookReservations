using BookReservations.Infrastructure.DAL.Query.Interfaces;

namespace BookReservations.Infrastructure.DAL;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;
    IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : Entity<TKey> where TKey : IEquatable<TKey>;
    IQuery<TEntity> Query<TEntity>() where TEntity : class, IBaseEntity;
    Task CommitAsync(CancellationToken cancellationToken = default);
}
