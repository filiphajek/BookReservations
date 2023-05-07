using BookReservations.Api.Client;
using BookReservations.App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.ViewModels;

public partial class UserCardViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;

    public UserCardViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    [ObservableProperty]
    private UserInfoModel user;

    [ObservableProperty]
    private bool isFlyoutOpen = false;

    [RelayCommand]
    public async Task GoToUserDetailAsync()
    {
        IsFlyoutOpen = false;
        await Shell.Current.GoToAsync(UserDetailPage.Route, new Dictionary<string, object>
        {
            [nameof(UserDetailViewModel.Id)] = User.Id
        });
    }

    [RelayCommand]
    private void HideFlyout() => IsFlyoutOpen = false;

    public async Task InitializeAsync()
    {
        User = (await apiClient.GetUserInfoAsync()).Result;
    }
}
