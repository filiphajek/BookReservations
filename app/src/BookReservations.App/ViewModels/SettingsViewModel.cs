using CommunityToolkit.Mvvm.ComponentModel;

namespace BookReservations.App.ViewModels;

public partial class SettingsViewModel : ObservableObject, IViewModel
{
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}