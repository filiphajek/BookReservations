using BookReservations.Api.Client;
using BookReservations.App.BL.Services;
using BookReservations.App.Messages;
using BookReservations.App.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(Username), nameof(Username))]
public partial class SignUpViewModel : ObservableObject, IViewModel
{
    private readonly static string defaultImage = "https://bookreservationsstorage.blob.core.windows.net/bookreservations/default.jpg";

    private readonly IApiClient apiClient;
    private readonly ILoginService loginService;
    private readonly IMessengerService messengerService;

    public SignUpViewModel(IApiClient apiClient, ILoginService loginService, IMessengerService messengerService)
    {
        this.apiClient = apiClient;
        this.loginService = loginService;
        this.messengerService = messengerService;
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
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
        {
            Error = "All fields are required";
            return;
        }

        try
        {
            var result = await apiClient.RegisterAsync(new()
            {
                Email = Email,
                UserName = Username,
                FirstName = FirstName,
                LastName = LastName,
                Image = defaultImage,
                Password = password
            });

            if (result.Result.Success)
            {
                var loginResult = await loginService.LoginAsync(Username, password);
                if (loginResult)
                {
                    messengerService.Send(new ChangeNavigationModeMessage());
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Error = ex switch
            {
                SwaggerException<CreateUserResponse> err => string.Join(Environment.NewLine, err.Result.Errors.First().Value),
                SwaggerException<ValidationErrorResponse> err => string.Join(Environment.NewLine, err.Result.Errors.First().Value),
                _ => "Unknown error, try again"
            };
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public Task InitializeAsync() => Task.CompletedTask;
}
