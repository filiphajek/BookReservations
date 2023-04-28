using CommunityToolkit.Mvvm.ComponentModel;

namespace BookReservations.App.ViewModels;

public partial class SignUpViewModel : ObservableObject, IViewModel
{
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
