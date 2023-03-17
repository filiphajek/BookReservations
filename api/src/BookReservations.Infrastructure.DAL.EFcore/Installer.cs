using BookReservations.Infrastructure.DAL.EFcore.Query;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace BookReservations.Infrastructure.DAL.EFcore;

public static class Installer
{
    public static IServiceCollection AddQueries(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime serviceLifetime)
    {
        var entities = assemblies.SelectMany(i => i.GetTypes().Where(i => typeof(IBaseEntity).IsAssignableFrom(i) && i.IsClass)).ToList();

        //register default implementations of query for each entity type
        foreach (var entity in entities)
        {
            var arg = GetArgumentOfGenericType(entity);
            var query = typeof(IQuery<>).MakeGenericType(entity);
            services.Add(new ServiceDescriptor(query, typeof(EfQuery<,>).MakeGenericType(entity, arg), serviceLifetime));
        }

        //override with custom implementations
        foreach (var entity in entities)
        {
            var query = typeof(IQuery<>).MakeGenericType(entity);

            services.Scan(selector =>
                selector.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(query)).UsingRegistrationStrategy(RegistrationStrategy.Replace(ReplacementBehavior.Default))
                .AsImplementedInterfaces()
                .WithLifetime(serviceLifetime));
        }

        return services;
    }

    public static IServiceCollection AddRepositories<TDbContext>(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime serviceLifetime)
        where TDbContext : DbContext
    {
        var entities = assemblies.SelectMany(i => i.GetTypes().Where(i => typeof(IBaseEntity).IsAssignableFrom(i) && i.IsClass)).ToList();
        var typeOfDbContext = typeof(TDbContext);

        //register default implementation of repository for each entity type
        foreach (var entity in entities)
        {
            var arg = GetArgumentOfGenericType(entity);
            var repositoryWithKey = typeof(IRepository<,>).MakeGenericType(entity, arg);

            if (arg == typeof(int))
            {
                var repositoryWithoutKey = typeof(IRepository<>).MakeGenericType(entity);
                services.Add(new ServiceDescriptor(repositoryWithoutKey, typeof(UoWRepository<,>).MakeGenericType(typeOfDbContext, entity), serviceLifetime));
            }
            services.Add(new ServiceDescriptor(repositoryWithKey, typeof(UoWRepository<,,>).MakeGenericType(typeOfDbContext, entity, arg), serviceLifetime));
        }

        //override with custom implementations
        foreach (var entity in entities)
        {
            var arg = GetArgumentOfGenericType(entity);
            var repositoryWithKey = typeof(IRepository<,>).MakeGenericType(entity, arg);

            services.Scan(selector =>
                selector.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(repositoryWithKey)).UsingRegistrationStrategy(RegistrationStrategy.Replace(ReplacementBehavior.Default))
                .AsImplementedInterfaces()
                .WithLifetime(serviceLifetime));
        }

        return services;
    }

    private static Type GetArgumentOfGenericType(Type type)
    {
        var baseType = type.BaseType;
        while (true)
        {
            var arg = baseType!.GetGenericArguments().SingleOrDefault();
            if (arg is not null)
            {
                return arg;
            }
            baseType = baseType.BaseType;
        }
    }
}
