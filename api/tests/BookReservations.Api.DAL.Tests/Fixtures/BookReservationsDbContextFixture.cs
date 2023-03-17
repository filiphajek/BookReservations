using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.EFcore;
using BookReservations.Infrastructure.Tests.Factories;
using BookReservations.Infrastructure.Tests.Fixtures;

namespace BookReservations.Api.DAL.Tests.Fixtures;

public class BookReservationsDbContextFixture : DbContextFixture<BookReservationsDbContext>
{
    public IUnitOfWork UnitOfWork { get; private set; } = default!;

    public BookReservationsDbContextFixture(IDbContextFactory<BookReservationsDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    protected override Task SeedAsync()
    {
        return Task.CompletedTask;
    }

    public override Task AfterInitializeAsync()
    {
        UnitOfWork = new EfUnitOfWork<BookReservationsDbContext>(DbContext);
        return base.AfterInitializeAsync();
    }
}