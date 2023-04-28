using CommunityToolkit.Mvvm.ComponentModel;

namespace BookReservations.App.ViewModels;

public partial class ReservationViewModel : ObservableObject, IViewModel
{
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
