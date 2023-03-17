using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.EFcore;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BookReservations.Api.DAL;

public class UnitOfWork : EfUnitOfWork<BookReservationsDbContext>
{
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWork(BookReservationsDbContext context, IServiceProvider serviceProvider) : base(context)
    {
        _serviceProvider = serviceProvider;
    }

    public override IRepository<TEntity> GetRepository<TEntity>()
    {
        return _serviceProvider.GetRequiredService<IRepository<TEntity>>();
    }

    public override IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
    {
        return _serviceProvider.GetRequiredService<IRepository<TEntity, TKey>>();
    }

    public override IQuery<TEntity> Query<TEntity>()
    {
        return _serviceProvider.GetRequiredService<IQuery<TEntity>>();
    }
}