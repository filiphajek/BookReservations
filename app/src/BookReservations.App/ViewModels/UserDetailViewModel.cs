using CommunityToolkit.Mvvm.ComponentModel;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class UserDetailViewModel : ObservableObject, IViewModel
{
    public int Id { get; set; }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
