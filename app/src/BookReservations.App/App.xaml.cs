using BookReservations.App.Messages;
using BookReservations.App.Services;

namespace BookReservations.App;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider, IMessengerService messengerService)
    {
        InitializeComponent();
        SetLanguage();

        messengerService.Register<ChangeNavigationModeMessage>(i =>
        {
            MainPage = serviceProvider.GetRequiredService<AppShell>();
        });
        MainPage = serviceProvider.GetRequiredService<LoginShell>();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

#if WINDOWS
        window.Width = 480;
        window.Height = 800;
#endif

        return window;
    }

    private static void SetLanguage()
    {
        var culture = "cs";
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
    }
}