using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class CatalogPage
{
    public const string Route = "catalog";

    public CatalogPage(CatalogViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}