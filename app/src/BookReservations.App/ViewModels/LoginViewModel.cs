using BookReservations.App.BL.Services;
using BookReservations.App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;

namespace BookReservations.App.ViewModels;

public partial class LoginViewModel : ObservableValidator, IViewModel
{
    private readonly ILoginService loginService;

    public LoginViewModel(ILoginService loginService)
    {
        this.loginService = loginService;
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
        var tmp = await loginService.LoginAsync(Username, password);

        await Shell.Current.GoToAsync("catalog");

        ValidateAllProperties();

        //todo validace .. CustomValidation https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/3750
        Error = string.Empty;
        if (HasErrors)
        {
            Error = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
        }
    }

    [RelayCommand]
    private async Task SignOnAsync()
    {
        await Shell.Current.GoToAsync(SignUpPage.Route);
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
