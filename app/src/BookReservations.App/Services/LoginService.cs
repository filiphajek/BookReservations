using BookReservations.Api.Client;
using BookReservations.App.BL.Services;
using IdentityModel.Client;

namespace BookReservations.App.Services;

public class LoginService : ILoginService
{
    private readonly IApiClient apiClient;
    private readonly ISecureStorage secureStorage;

    public LoginService(IApiClient apiClient, ISecureStorage secureStorage)
    {
        this.apiClient = apiClient;
        this.secureStorage = secureStorage;
    }

    public async Task<bool> LoginAsync(string login, string password)
    {
        try
        {
            var response = await apiClient.LoginJwtAsync(new()
            {
                Login = login,
                Password = password
            });
            if (response.Result.Success)
            {
                await secureStorage.SetAsync("token", response.Result.Token);
                ((ApiClient)apiClient).HttpClient.SetBearerToken(response.Result.Token);
            }

            return response.Result.Success;
        }
        catch
        {
            return false;
        }
    }

    public async Task TryAuthorizeAsync()
    {
        var token = await secureStorage.GetAsync("token");
        // todo try call userinfo 
        if (!string.IsNullOrEmpty(token))
        {
            ((ApiClient)apiClient).HttpClient.SetBearerToken(token);
        }
    }
}