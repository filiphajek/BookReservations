using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class BookPage
{
    public BookPage(BookViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}