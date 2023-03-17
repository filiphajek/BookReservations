using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.EFcore;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.DAL;

public class UnitOfWorkProvider : EfUnitOfWorkProvider<BookReservationsDbContext>
{
    private readonly IServiceProvider serviceProvider;

    public UnitOfWorkProvider(
        IUnitOfWork unitOfWork,
        IServiceProvider serviceProvider,
        IDbContextFactory<BookReservationsDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
        this.serviceProvider = serviceProvider;
        UnitOfWork = unitOfWork;
    }

    protected override IUnitOfWork CreateUnitOfWork(BookReservationsDbContext context)
    {
        return new UnitOfWork(context, serviceProvider);
    }
}
