using BookReservations.App.BL.Services;
using BookReservations.App.Messages;
using BookReservations.App.Services;
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
        messengerService.Send(new LoginMesage(tmp));

        ValidateAllProperties();

        //todo validace .. CustomValidation https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/3750
        Error = string.Empty;
        if (HasErrors)
        {
            Error = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;
}
