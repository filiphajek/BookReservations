namespace BookReservations.Infrastructure.DAL;

public interface IRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : IEquatable<TKey>
{
    IQueryable<TEntity> GetQueryable();
    Task<ICollection<TEntity>> GetAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> SingleAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> InsertOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity?> SingleByIdAsync(TKey id, CancellationToken cancellationToken = default);
}

public interface IRepository<TEntity> : IRepository<TEntity, int>
    where TEntity : Entity
{
}
