using BookReservations.App.ViewModels;
using CommunityToolkit.Maui.Views;

namespace BookReservations.App.Views;

public partial class AddReservationPopup : Popup
{
    public AddReservationPopup()
    {
        InitializeComponent();
        BindingContext = new AddReservationViewModel();
    }

    private void OnReservationCreate(object sender, EventArgs e)
    {
        Close(true);
    }
}