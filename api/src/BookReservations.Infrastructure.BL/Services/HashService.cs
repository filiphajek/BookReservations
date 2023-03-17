namespace BookReservations.Infrastructure.BL.Services;

public class HashService : IHashService
{
    public string Hash(string text, string? salt = null)
    {
        if (salt is not null)
        {
            return BCrypt.Net.BCrypt.HashPassword(text, salt);
        }

        return BCrypt.Net.BCrypt.HashPassword(text);
    }

    public bool Verify(string text, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(text, hash);
    }
}
