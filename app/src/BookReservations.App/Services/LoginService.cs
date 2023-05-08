using BookReservations.Api.Client;
using BookReservations.App.BL.Services;
using BookReservations.App.Msal;
using IdentityModel.Client;
using Microsoft.Identity.Client;

namespace BookReservations.App.Services;

public class LoginService : ILoginService
{
    private readonly IApiClient apiClient;
    private readonly ISecureStorage secureStorage;
    private readonly MsPublicClient msClient;

    public LoginService(IApiClient apiClient, ISecureStorage secureStorage, MsPublicClient msClient)
    {
        this.apiClient = apiClient;
        this.secureStorage = secureStorage;
        this.msClient = msClient;
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

    public async Task<bool> MsSignOnAsync()
    {
        try
        {
            var result = await msClient.AcquireTokenSilentAsync(MsPublicClient.Scopes).ConfigureAwait(false);
            return await MsSignOnAsync(result.AccessToken);
        }
        catch (MsalUiRequiredException)
        {
            try
            {
                var result = await msClient.AcquireTokenInteractiveAsync(MsPublicClient.Scopes).ConfigureAwait(false);
                return await MsSignOnAsync(result.AccessToken);
            }
            catch { }
        }
        return false;
    }

    private async Task<bool> MsSignOnAsync(string token)
    {
        ((ApiClient)apiClient).HttpClient.SetBearerToken(token);
        var msSignOn = await apiClient.MsSignOnAsync();
        if (msSignOn.Result.Success)
        {
            await secureStorage.SetAsync("token", msSignOn.Result.Token);
            ((ApiClient)apiClient).HttpClient.SetBearerToken(msSignOn.Result.Token);
        }
        return msSignOn.Result.Success;
    }

    public async Task MsSignOutAsync()
    {
        try
        {
            await msClient.SignOutAsync();
        }
        catch { }
    }

    public async Task<bool> TryAuthorizeAsync()
    {
        var token = await secureStorage.GetAsync("token");
        if (!string.IsNullOrEmpty(token))
        {
            ((ApiClient)apiClient).HttpClient.SetBearerToken(token);
            try
            {
                await apiClient.GetUserInfoAsync();
            }
            catch { }
            return true;
        }
        return false;
    }
}