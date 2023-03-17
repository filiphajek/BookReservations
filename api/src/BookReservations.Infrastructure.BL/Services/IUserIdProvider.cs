namespace BookReservations.Infrastructure.BL.Services;

public interface IUserIdProvider : IService
{
    int GetUserId();
}
