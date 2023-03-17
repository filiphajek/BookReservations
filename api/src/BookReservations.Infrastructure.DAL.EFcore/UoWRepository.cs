using Microsoft.EntityFrameworkCore;

namespace BookReservations.Infrastructure.DAL.EFcore;

public class UoWRepository<TDbContext, TEntity> : EfRepository<TDbContext, TEntity>
    where TDbContext : DbContext
    where TEntity : Entity
{
    private readonly IUnitOfWorkProvider provider;
    protected override TDbContext Context => ((EfUnitOfWork<TDbContext>)provider.UnitOfWork).Context;

    public UoWRepository(IUnitOfWorkProvider provider, TDbContext context) : base(context)
    {
        this.provider = provider;
    }
}

public class UoWRepository<TDbContext, TEntity, TKey> : EfRepository<TDbContext, TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : Entity<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly IUnitOfWorkProvider provider;
    protected override TDbContext Context => ((EfUnitOfWork<TDbContext>)provider.UnitOfWork).Context;

    public UoWRepository(IUnitOfWorkProvider provider, TDbContext context) : base(context)
    {
        this.provider = provider;
    }
}