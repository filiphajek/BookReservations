using BookReservations.Api.DAL.Tests.Factories;
using BookReservations.Infrastructure.DAL.EFcore;
using BookReservations.Infrastructure.Tests.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookReservations.Api.DAL.Tests;

public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder)
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        hostBuilder.ConfigureHostConfiguration(builder => builder.AddConfiguration(config));
    }

    public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        var env = context.Configuration.GetValue<string>("TEST_ENVIRONMENT");

        services.AddSingleton<ISeeder, TestSeeder>();
        services.AddSingleton<IDbContextFactory<BookReservationsDbContext>, MsSqlDbContextFactory>();

        if (env == "actions")
        {
            services.AddSingleton(new DbContextFactoryOptions("Server=localhost,1433;User Id=SA;Password=Sql12345*;TrustServerCertificate=True;"));
        }
        else
        {
            services.AddSingleton(new DbContextFactoryOptions("Server=(localdb)\\mssqllocaldb;Integrated Security=True;Trusted_Connection=True;"));
        }
    }
}
