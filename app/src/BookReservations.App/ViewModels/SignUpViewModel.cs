using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(Username), nameof(Username))]
public partial class SignUpViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;

    public SignUpViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    [ObservableProperty]
    private string username = "";

    [ObservableProperty]
    private string email = "";

    [ObservableProperty]
    private string firstName = "";

    [ObservableProperty]
    private string lastName = "";

    [RelayCommand]
    private async Task SignUpAsync()
    {
        var result = await apiClient.RegisterAsync(new()
        {
            Email = Email,
            UserName = Username,
            FirstName = FirstName,
            LastName = LastName,
            Image = ""
        });
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
