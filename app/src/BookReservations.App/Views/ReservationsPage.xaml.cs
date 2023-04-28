using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class ReservationsPage
{
    public ReservationsPage(ReservationViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}