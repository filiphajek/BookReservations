using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.EFcore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BookReservations.Api.DAL;

public static class DALInstaller
{
    public static IServiceCollection AddDatabaseLayer(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<DatabaseOptions>(configuration.GetSection(nameof(DatabaseOptions)))
            .AddUnitOfWork(configuration)
            .AddRepositories()
            .AddQueries();
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<BookReservationsDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("Default")))
            .AddDbContextFactory<BookReservationsDbContext>(lifetime: ServiceLifetime.Scoped)
            .AddScoped(provider => provider.GetRequiredService<BookReservationsDbContext>().Model)
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IUnitOfWorkProvider, UnitOfWorkProvider>()
            .AddSingleton<ISeeder, Seeder>();
    }

    public static IServiceCollection AddQueries(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services.AddQueries(new[] { assembly }, serviceLifetime);
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services.AddRepositories<BookReservationsDbContext>(new[] { assembly }, serviceLifetime);
    }

    public static IServiceCollection AddRepositoriesFromAssembly<TAssembly>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var assembly = typeof(TAssembly).Assembly;
        return services.AddRepositories<BookReservationsDbContext>(new[] { assembly }, serviceLifetime);
    }

    public static IServiceCollection AddRepositoriesFromAllAssemblies(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return services.AddRepositories<BookReservationsDbContext>(assemblies, serviceLifetime);
    }
}
