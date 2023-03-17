using BookReservations.Infrastructure.DAL.EFcore;
using BookReservations.Infrastructure.Tests.Factories;
using BookReservations.Infrastructure.Tests.Fixtures.MockedAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace BookReservations.Infrastructure.Tests.Fixtures;

public class ApiTestsFixture<T, TDbContext, TSeeder> : IAsyncLifetime
    where T : class
    where TDbContext : DbContext
    where TSeeder : class, ISeeder
{
    public HttpClient HttpClient { get; }
    public virtual MockedClaims Claims => new();

    protected readonly IServiceProvider serviceProvider;
    private readonly string dbName = Guid.NewGuid().ToString();

    public ApiTestsFixture(DbContextFactoryOptions dbOptions)
    {
        var appFactory = new WebApplicationFactory<T>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(TDbContext));
                    services.RemoveAll(typeof(DbContextOptions<TDbContext>));
                    services.RemoveAll(typeof(ISeeder));
                    services.AddSingleton<ISeeder, TSeeder>();

                    services.AddDbContext<TDbContext>(options =>
                    {
                        options.UseSqlServer(dbOptions.ConnectionString + $"Database={dbName};");
                    });
                    services.AddDbContextFactory<TDbContext>();
                });
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(Claims);
                    services.AddSingleton<IAuthenticationSchemeProvider, MockSchemeProvider>();
                });
            });

        serviceProvider = appFactory.Services;
        HttpClient = appFactory.CreateDefaultClient();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await context.Database.EnsureDeletedAsync();
    }
}
