using BookReservations.Api.Client;
using BookReservations.App.BL.Services;
using BookReservations.App.Services;
using BookReservations.App.ViewModels;
using BookReservations.App.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BookReservations.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton(SecureStorage.Default);
        builder.Services.AddSingleton<IMessengerService, MessengerService>();

        builder.Services.AddSingleton<IApiClient>(_ => new ApiClient("https://localhost:7026/", new HttpClient()));

        builder.Services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<BaseContentPage>())
            .AsSelf()
            .WithTransientLifetime());

        builder.Services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<IViewModel>())
            .AsSelfWithInterfaces()
            .WithTransientLifetime());

        builder.Services.Scan(selector => selector
            .FromAssemblies(typeof(IService).Assembly, typeof(App).Assembly)
            .AddClasses(filter => filter.AssignableTo<IService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}