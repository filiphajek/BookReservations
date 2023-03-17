using BookReservations.Infrastructure.DAL.EFcore;
using BookReservations.Infrastructure.Tests.Factories;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.DAL.Tests.Factories;

public class MsSqlDbContextFactory : Infrastructure.Tests.Factories.IDbContextFactory<BookReservationsDbContext>
{
    private readonly ISeeder seeder;
    private readonly DbContextFactoryOptions options;

    public MsSqlDbContextFactory(ISeeder seeder, DbContextFactoryOptions options)
    {
        this.seeder = seeder;
        this.options = options;
    }

    public BookReservationsDbContext CreateDbContext(string dbName)
    {
        var connString = options.ConnectionString + $"Database={dbName};";
        return new BookReservationsDbContext(new DbContextOptionsBuilder<BookReservationsDbContext>()
            .UseSqlServer(connString, builder =>
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)).Options, seeder);
    }
}
