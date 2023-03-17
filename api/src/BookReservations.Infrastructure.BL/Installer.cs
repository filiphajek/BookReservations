using Azure.Storage.Blobs;
using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.BL.Services;
using BookReservations.Infrastructure.DAL;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BookReservations.Infrastructure.BL;

public static class Installer
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddMediatR(assemblies);
        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services, Assembly[] assemblies)
    {
        var config = new TypeAdapterConfig();
        config.Scan(assemblies);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services,
        Assembly[] assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        return services.Scan(selector =>
            selector.FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IService)))
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));
    }

    public static IServiceCollection AddFacades(this IServiceCollection services,
        Assembly[] assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        return services.Scan(selector =>
            selector.FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IFacade)))
            .AsImplementedInterfaces()
            .WithLifetime(serviceLifetime));
    }

    public static IServiceCollection AddAllRequestHandlers<TModel, TEntity>(this IServiceCollection services)
        where TEntity : class, IBaseEntity
    {
        return services.AddSetCommandHandler<TModel, TEntity>()
                       .AddDeleteCommandHandler<TModel, TEntity>()
                       .AddSimpleQueryHandler<TModel, TEntity>()
                       .AddPaginatedQueryHandler<TModel, TEntity>();
    }

    public static IServiceCollection AddSetCommandHandler<TModel, TEntity>(this IServiceCollection services)
        where TEntity : class, IBaseEntity
    {
        var request = typeof(SetCommand<>).MakeGenericType(typeof(TModel));
        var response = typeof(ICollection<>).MakeGenericType(typeof(TModel));

        return services.AddRequestHandler<TModel, TEntity>(request, response, typeof(SetCommandHandler<,,>));
    }

    public static IServiceCollection AddDeleteCommandHandler<TModel, TEntity>(this IServiceCollection services)
        where TEntity : class, IBaseEntity
    {
        var request = typeof(DeleteCommand<>).MakeGenericType(typeof(TEntity));
        var response = typeof(Unit);

        return services.AddRequestHandler<TModel, TEntity>(request, response, typeof(DeleteCommandHandler<,,>));
    }

    public static IServiceCollection AddSimpleQueryHandler<TModel, TEntity>(this IServiceCollection services)
        where TEntity : class, IBaseEntity
    {
        var request = typeof(SimpleQuery<,>).MakeGenericType(typeof(TModel), typeof(TEntity));
        var response = typeof(ICollection<>).MakeGenericType(typeof(TModel));

        return services.AddRequestHandler<TModel, TEntity>(request, response, typeof(SimpleQueryHandler<,,>));
    }

    public static IServiceCollection AddPaginatedQueryHandler<TModel, TEntity>(this IServiceCollection services)
        where TEntity : class, IBaseEntity
    {
        var request = typeof(PaginatedQuery<,>).MakeGenericType(typeof(TModel), typeof(TEntity));
        var response = typeof(PaginatedQueryResult<>).MakeGenericType(typeof(TModel));

        var requestHandlerInterface = typeof(IRequestHandler<,>).MakeGenericType(request, response);
        var requestHandler = typeof(PaginatedQueryHandler<,,,>).MakeGenericType(
            request, typeof(TModel), typeof(TEntity), GetArgumentOfGenericType(typeof(TEntity)));

        return services.TryAddRequestHandler(requestHandlerInterface, requestHandler);
    }

    public static IServiceCollection AddFileService(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<AzureBlobStorageOptions>(configuration.GetSection(nameof(AzureBlobStorageOptions)))
            .AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<AzureBlobStorageOptions>>().Value;
                return new BlobContainerClient(options.ConnectionString, options.ContainerName);
            })
            .AddScoped<IFileStorageService, AzureStorageService>();
    }

    private static IServiceCollection AddRequestHandler<TModel, TEntity>(
        this IServiceCollection services, Type request, Type response, Type requestHandlerType)
    {
        var modelType = typeof(TModel);
        var entityType = typeof(TEntity);

        var requestHandlerInterface = typeof(IRequestHandler<,>).MakeGenericType(request, response);
        var requestHandler = requestHandlerType.MakeGenericType(modelType, entityType, GetArgumentOfGenericType(entityType));

        return services.TryAddRequestHandler(requestHandlerInterface, requestHandler);
    }

    private static IServiceCollection TryAddRequestHandler(this IServiceCollection services, Type requestHandlerInterface, Type requestHandler)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        if (!assemblies.Any(i => i.GetTypes().Any(i => requestHandler.IsAssignableFrom(i) && i.IsClass)))
        {
            services.AddScoped(requestHandlerInterface, requestHandler);
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