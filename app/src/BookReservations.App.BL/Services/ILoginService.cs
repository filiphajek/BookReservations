namespace BookReservations.App.BL.Services;

public interface ILoginService : IService
{
    Task<bool> TryAuthorizeAsync();
    Task<bool> LoginAsync(string login, string password);
}
