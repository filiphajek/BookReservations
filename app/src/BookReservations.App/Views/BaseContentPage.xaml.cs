using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class BaseContentPage : IView
{
    public IViewModel ViewModel { get; }

    public BaseContentPage(IViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = ViewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.InitializeAsync();
    }
}