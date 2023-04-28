using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class SignUpPage
{
    public const string Route = "signup";

    public SignUpPage(SignUpViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}