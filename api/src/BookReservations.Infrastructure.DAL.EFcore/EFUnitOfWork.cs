using BookReservations.Infrastructure.DAL.EFcore.Query;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Infrastructure.DAL.EFcore;

public class EfUnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
{
    public TDbContext Context { get; }

    public EfUnitOfWork(TDbContext context)
    {
        Context = context;
    }

    public virtual IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
    {
        return new EfRepository<TDbContext, TEntity>(Context);
    }

    public virtual IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    {
        return new EfRepository<TDbContext, TEntity, TKey>(Context);
    }

    public virtual IQuery<TEntity> Query<TEntity>() where TEntity : class, IBaseEntity
    {
        var query = Context.Set<TEntity>().AsNoTracking();
        return new EfQuery<TEntity>(query, Context.Model);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
    }
}