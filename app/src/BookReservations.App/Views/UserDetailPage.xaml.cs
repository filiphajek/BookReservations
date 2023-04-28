using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class UserDetailPage
{
    public const string Route = "users/detail";

    public UserDetailPage(UserDetailViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}