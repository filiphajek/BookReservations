using Microsoft.EntityFrameworkCore;

namespace BookReservations.Infrastructure.DAL.EFcore;

public class EfRepository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : Entity<TKey>
    where TKey : IEquatable<TKey>
{
    protected virtual TDbContext Context { get; }
    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    public EfRepository(TDbContext context)
    {
        Context = context;
    }

    public virtual async Task<ICollection<TEntity>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await Set.ToListAsync(cancellationToken);
    }

    public virtual async Task<bool> ExistsAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return await ExistsByIdAsync(entity.Id, cancellationToken);
    }

    public virtual async Task<TEntity?> SingleAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return await SingleByIdAsync(entity.Id, cancellationToken);
    }

    public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (await ExistsAsync(entity, cancellationToken))
        {
            return await UpdateAsync(entity, cancellationToken);
        }
        else
        {
            return await InsertAsync(entity, cancellationToken);
        }
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var newEntity = await Set.AddAsync(entity, cancellationToken);
        return newEntity.Entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
        return await Task.FromResult(entity);
    }

    public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entityToDelete = await SingleAsync(entity, cancellationToken);

        if (entityToDelete is null)
        {
            return;
        }
        if (Context.Entry(entityToDelete).State == EntityState.Detached)
        {
            Context.Attach(entityToDelete);
        }

        Set.Remove(entityToDelete);
    }

    public virtual async Task<bool> ExistsByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        if (id != null)
        {
            return await Set.AnyAsync(i => i.Id.Equals(id), cancellationToken);
        }
        return false;
    }

    public virtual async Task<TEntity?> SingleByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var foundEntity = await Set.FirstOrDefaultAsync(i => i.Id.Equals(id), cancellationToken);
        return foundEntity;
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return Set.AsNoTracking();
    }
}

public class EfRepository<TDbContext, TEntity> : EfRepository<TDbContext, TEntity, int>, IRepository<TEntity>
    where TDbContext : DbContext
    where TEntity : Entity
{
    public EfRepository(TDbContext context) : base(context)
    {
    }
}
