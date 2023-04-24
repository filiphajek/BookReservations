using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class CatalogPage
{
    public CatalogPage(CatalogViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}