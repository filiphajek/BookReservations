
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Infrastructure.Tests.Fixtures;

public abstract class DbContextFixture<TDbContext> : Fixture
    where TDbContext : DbContext
{
    protected readonly Factories.IDbContextFactory<TDbContext> dbContextFactory;
    protected readonly string dbName = Guid.NewGuid().ToString();

    public TDbContext DbContext { get; private set; } = default!;

    protected DbContextFixture(Factories.IDbContextFactory<TDbContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public override async Task InitializeAsync()
    {
        DbContext = dbContextFactory.CreateDbContext(dbName);
        await DbContext.Database.EnsureCreatedAsync();
        await base.InitializeAsync();
        await AfterInitializeAsync();
    }

    public virtual Task AfterInitializeAsync()
    {
        return Task.CompletedTask;
    }

    public override async Task DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.DisposeAsync();
    }
}
