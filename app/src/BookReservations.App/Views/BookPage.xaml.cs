using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class BookPage
{
    public const string Route = "books/detail";

    public BookPage(BookViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}