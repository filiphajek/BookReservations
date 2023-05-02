using BookReservations.App.BL.Services;
using BookReservations.App.Messages;
using BookReservations.App.Services;
using BookReservations.App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace BookReservations.App.ViewModels;

public partial class LoginViewModel : ObservableValidator, IViewModel
{
    private readonly ILoginService loginService;
    private readonly IMessengerService messengerService;

    public LoginViewModel(ILoginService loginService, IMessengerService messengerService)
    {
        this.loginService = loginService;
        this.messengerService = messengerService;
    }

    [MinLength(3)]
    [ObservableProperty]
    private string username = "";

    [ObservableProperty]
    private string _error;

    private bool CanLogin()
    {
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            Error = "Enter the password";
            return;
        }

        var result = await loginService.LoginAsync(Username, password);

        if (result)
        {
            messengerService.Send(new ChangeNavigationModeMessage());
            return;
        }

        // CustomValidation https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/3750
        Error = "Wrong password or username";
    }

    [RelayCommand]
    private async Task SignOnAsync()
    {
        await Shell.Current.GoToAsync(SignUpPage.Route, new Dictionary<string, object>()
        {
            { nameof(SignUpViewModel.Username),  Username }
        });
    }

    [RelayCommand]
    private async Task MsSignOnAsync()
    {
        await Task.Delay(10);
    }

    [RelayCommand]
    private async Task GoogleSignOnAsync()
    {
        await Task.Delay(10);
    }

    public Task InitializeAsync() => Task.CompletedTask;
}
