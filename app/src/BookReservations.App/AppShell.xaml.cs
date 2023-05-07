using BookReservations.App.ViewModels;

namespace BookReservations.App;

public partial class AppShell : Shell
{
    private readonly IViewModel _viewModel;

    public AppShell(UserCardViewModel userViewModel)
    {
        InitializeComponent();
        _viewModel = userViewModel;
        BindingContext = userViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

    private async void AboutButtonClicked(object sender, EventArgs e)
    {
        this.FlyoutIsPresented = false;
        await Current.GoToAsync("about");
    }
}