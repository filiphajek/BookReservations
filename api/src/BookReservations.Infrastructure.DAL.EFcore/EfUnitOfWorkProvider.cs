using Microsoft.EntityFrameworkCore;

namespace BookReservations.Infrastructure.DAL.EFcore;

public class EfUnitOfWorkProvider<TDbContext> : IUnitOfWorkProvider where TDbContext : DbContext
{
    protected readonly IDbContextFactory<TDbContext> dbContextFactory;

    public IUnitOfWork UnitOfWork { get; protected set; } = default!;

    public EfUnitOfWorkProvider(IDbContextFactory<TDbContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<IUnitOfWork> CreateAsync()
    {
        var context = await dbContextFactory.CreateDbContextAsync();
        UnitOfWork = CreateUnitOfWork(context);
        return UnitOfWork;
    }

    public IUnitOfWork Create()
    {
        var context = dbContextFactory.CreateDbContext();
        UnitOfWork = CreateUnitOfWork(context);
        return UnitOfWork;
    }

    protected virtual IUnitOfWork CreateUnitOfWork(TDbContext context)
    {
        return new EfUnitOfWork<TDbContext>(context);
    }
}
