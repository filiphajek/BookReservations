using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.ViewModels;

public partial class AddReservationViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTime minFromDate = DateTime.Now.AddDays(1);

    [ObservableProperty]
    private DateTime maxFromDate = DateTime.Now.AddDays(60);

    [ObservableProperty]
    private DateTime fromDate = DateTime.Now.AddDays(1);

    [ObservableProperty]
    private DateTime minToDate = DateTime.Now.AddDays(2);

    [ObservableProperty]
    private DateTime toDate = DateTime.Now.AddDays(5);

    public AddReservationViewModel()
    {
        PropertyChanged += FromDateChanged;
    }

    private void FromDateChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(FromDate))
        {
            MinToDate = FromDate.AddDays(1);
            ToDate = FromDate.AddDays(1);
        }
    }
}
