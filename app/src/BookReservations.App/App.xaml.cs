using BookReservations.App.Messages;
using BookReservations.App.Services;
using BookReservations.App.ViewModels;
using BookReservations.App.Views;

namespace BookReservations.App;

public partial class App : Application
{
    public App(IMessengerService messengerService, LoginViewModel vm)
    {
        InitializeComponent();
        messengerService.Register<LoginMesage>(i =>
        {
            MainPage = new AppShell();
        });
        MainPage = new LoginPage(vm);
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Width = 480;
        window.Height = 800;

        return window;
    }
}