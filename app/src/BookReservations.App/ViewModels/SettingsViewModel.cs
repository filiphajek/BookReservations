using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookReservations.App.ViewModels;

public partial class SettingsViewModel : ObservableObject, IViewModel
{
    private readonly IPreferences preferences;

    public SettingsViewModel(IPreferences preferences)
    {
        this.preferences = preferences;
        PropertyChanged += SettingsViewModelPropertyChanged;
    }

    private async void SettingsViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(SelectedLanguage))
        {
            preferences.Set("booksres-language", SelectedLanguage.ToLower());
            var isInitializing = SelectedLanguage.ToLower() == Thread.CurrentThread.CurrentCulture.Name.ToLower();
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(SelectedLanguage.ToLower());
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SelectedLanguage.ToLower());
            if (!isInitializing)
            {
                await Toast.Make("Restart the app", ToastDuration.Long).Show();
            }
        }
    }

    [ObservableProperty]
    private string selectedLanguage = "";

    public Task InitializeAsync()
    {
        SelectedLanguage = preferences.Get("booksres-language", "EN").ToUpper();
        return Task.CompletedTask;
    }
}