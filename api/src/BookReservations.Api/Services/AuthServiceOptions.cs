namespace BookReservations.Api.Services;

public class AuthServiceOptions
{
    public bool IsPersistent { get; set; } = false;
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
