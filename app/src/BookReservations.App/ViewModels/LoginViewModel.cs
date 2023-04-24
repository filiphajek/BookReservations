using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.Client;
using System.ComponentModel.DataAnnotations;

namespace BookReservations.App.ViewModels;

public partial class LoginViewModel : ObservableValidator, IViewModel
{
    private readonly ISecureStorage secureStorage;
    private readonly IApiClient apiClient;

    public LoginViewModel(ISecureStorage secureStorage, IApiClient apiClient)
    {
        this.secureStorage = secureStorage;
        this.apiClient = apiClient;
    }

    [Required(ErrorMessage = "Username is required!")]
    [MinLength(2)]
    [ObservableProperty]
    private string username = "";

    [ObservableProperty]
    private string _error;

    private bool CanLogin => !string.IsNullOrEmpty(Username);

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync(string password)
    {
        var tmp = await apiClient.LoginJwtAsync(new()
        {
            Login = Username,
            Password = password
        });

        // todo service .. BL layer
        if (tmp.Result.Success)
        {
            await secureStorage.SetAsync("token", tmp.Result.Token);
            ((ApiClient)apiClient).HttpClient.SetBearerToken(tmp.Result.Token);
        }

        ValidateAllProperties();

        Error = string.Empty;
        if (HasErrors)
        {
            Error = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
        }

        //var tmp = (GetErrors()
        //    .ToDictionary(k => k.MemberNames.First(), v => v.ErrorMessage)
        //    ?? new Dictionary<string, string?>()).TryGetValue(nameof(Text), out var error);

        await Task.Delay(10);
    }

    public Task InitializeAsync() => Task.CompletedTask;
}
