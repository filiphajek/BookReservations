﻿using BookReservations.Api.Client;
using BookReservations.App.Messages;
using BookReservations.App.Services;
using BookReservations.App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.ViewModels;

public partial class UserCardViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;
    private readonly ISecureStorage secureStorage;

    public UserCardViewModel(IApiClient apiClient, ISecureStorage secureStorage, IMessengerService messengerService)
    {
        this.apiClient = apiClient;
        this.secureStorage = secureStorage;
        messengerService.Register<UserProfileChanged>(i =>
        {
            User = i.User;
        });
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

    [RelayCommand]
    private async Task LogoutAsync()
    {
        secureStorage.Remove("token");
        await Shell.Current.GoToAsync("login");
    }

    public async Task InitializeAsync()
    {
        User = (await apiClient.GetUserInfoAsync()).Result;
    }
}
