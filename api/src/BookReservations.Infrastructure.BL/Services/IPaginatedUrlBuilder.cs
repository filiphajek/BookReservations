namespace BookReservations.Infrastructure.BL.Services;

public interface IPaginatedUrlBuilder : IService
{
    string GetPaginatedUrl(int page);
}
