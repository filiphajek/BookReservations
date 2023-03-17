namespace BookReservations.Infrastructure.BL.Services;

public interface IHashService : IService
{
    string Hash(string text, string? salt = null);
    bool Verify(string text, string hash);
}
