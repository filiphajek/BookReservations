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

    private void SettingsViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(SelectedLanguage))
        {
            preferences.Set("booksres-language", SelectedLanguage.ToLower());
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(SelectedLanguage.ToLower());
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SelectedLanguage.ToLower());
        }
    }

    [ObservableProperty]
    private string selectedLanguage = "";

    public Task InitializeAsync()
    {
        SelectedLanguage = "";
        return Task.CompletedTask;
    }
}