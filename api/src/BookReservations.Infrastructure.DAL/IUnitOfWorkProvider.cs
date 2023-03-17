namespace BookReservations.Infrastructure.DAL;

public interface IUnitOfWorkProvider
{
    IUnitOfWork UnitOfWork { get; }
    IUnitOfWork Create();
    Task<IUnitOfWork> CreateAsync();
}
