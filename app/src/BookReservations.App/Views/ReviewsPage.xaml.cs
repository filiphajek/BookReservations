using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class ReviewsPage
{
    public const string Route = "books/detail/reviews";

    public ReviewsPage(BookViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}