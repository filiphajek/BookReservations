using BookReservations.Api.Client;
using BookReservations.App.BL.Services;
using BookReservations.App.Resources.Fonts;
using BookReservations.App.Services;
using BookReservations.App.ViewModels;
using BookReservations.App.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

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
                fonts.AddFont("FontAwesome-Solid.ttf", Fonts.FontAwesome);
                fonts.AddFont("OpenSans-Regular.ttf", Fonts.OpenSansRegular);
                fonts.AddFont("OpenSans-Semibold.ttf", Fonts.OpenSansSemibold);
            });

        var assembly = Assembly.GetExecutingAssembly();
        using var appSettingsStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".appsettings.json");
        builder.Configuration.AddJsonStream(appSettingsStream);

        builder.Services.AddSingleton(SecureStorage.Default);
        builder.Services.AddSingleton<IMessengerService, MessengerService>();
        builder.Services.AddSingleton<IApiClient>(_ => new ApiClient(builder.Configuration["ApiUrl"], new HttpClient()));
        builder.Services.AddTransient<AppShell>();
        builder.Services.AddTransient<LoginShell>();

        builder.Services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<Views.IView>())
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

        var routingPages = assembly
            .GetTypes()
            .Where(i => i.IsAssignableTo(typeof(Views.IView)))
            .Select(i => (i.GetField(nameof(SignUpPage.Route))?.GetValue(i)?.ToString(), i))
            .Where(i => !string.IsNullOrEmpty(i.Item1));

        foreach (var (route, page) in routingPages)
        {
            Routing.RegisterRoute(route, page);
        }
        Routing.RegisterRoute("about", typeof(AboutPage));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}