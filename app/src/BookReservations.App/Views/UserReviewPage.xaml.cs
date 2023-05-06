using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class UserReviewPage
{
    public const string Route = "books/detail/reviews/add";

    public UserReviewPage(UserReviewViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}