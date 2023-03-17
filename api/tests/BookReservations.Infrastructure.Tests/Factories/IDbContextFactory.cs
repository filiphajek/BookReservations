using Microsoft.EntityFrameworkCore;

namespace BookReservations.Infrastructure.Tests.Factories;

public interface IDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    TDbContext CreateDbContext(string dbConnectionString);
}
