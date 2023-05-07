using BookReservations.App.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace BookReservations.App.Views;

public partial class UserDetailPage
{
    public const string Route = "//users/detail";

    public UserDetailPage(UserDetailViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        App.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        App.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
    }
}