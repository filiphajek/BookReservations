using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class LoginPage
{
    public const string Route = "login";

    public LoginPage(LoginViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}