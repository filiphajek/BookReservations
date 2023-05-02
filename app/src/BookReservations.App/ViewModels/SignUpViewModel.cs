using BookReservations.Api.Client;
using BookReservations.App.BL.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(Username), nameof(Username))]
public partial class SignUpViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;
    private readonly ILoginService loginService;

    public SignUpViewModel(IApiClient apiClient, ILoginService loginService)
    {
        this.apiClient = apiClient;
        this.loginService = loginService;
    }

    [ObservableProperty]
    private string username = "";

    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string firstName = "";

    [ObservableProperty]
    private string lastName = "";

    [ObservableProperty]
    private string error = "";

    [RelayCommand]
    private async Task SignUpAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            Error = "Enter the password";
            return;
        }

        var result = await apiClient.RegisterAsync(new()
        {
            Email = Email,
            UserName = Username,
            FirstName = FirstName,
            LastName = LastName,
            Image = ""
        });

        if (result.Result.EmailExists)
        {
            Error = "Email already exists"; // todo nebo vzit jen 1. error
            return;
        }

        if (result.Result.Success)
        {
            var loginResult = await loginService.LoginAsync(Username, password);
            if (loginResult)
            {
                await Shell.Current.GoToAsync("catalog");
            }
        }
    }

    [RelayCommand]
    private async Task SignOnAsync()
    {
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public Task InitializeAsync() => Task.CompletedTask;
}
