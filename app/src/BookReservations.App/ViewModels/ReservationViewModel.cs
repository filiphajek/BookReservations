using BookReservations.Api.Client;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookReservations.App.ViewModels;

public partial class ReservationViewModel : ObservableObject, IViewModel
{
    public List<ReservationStatus> Statuses => new[] { (ReservationStatus)100 }.Concat((ReservationStatus[])Enum.GetValues(typeof(ReservationStatus))).ToList();

    private readonly IApiClient apiClient;

    public ReservationViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    [ObservableProperty]
    private ReservationStatus selectedStatus = (ReservationStatus)100;

    [ObservableProperty]
    private ObservableCollection<ReservationModel> reservations = new();

    [RelayCommand]
    private Task FilterReservationStatusAsync()
    {
        if (SelectedStatus == (ReservationStatus)100)
        {
            return LoadReservationsAsync();
        }
        return LoadReservationsAsync(SelectedStatus);
    }

    [RelayCommand]
    private async Task GoToBookDetailAsync(int bookId)
    {
        await Shell.Current.GoToAsync("//books/detail", new Dictionary<string, object>()
        {
            [nameof(BookViewModel.Id)] = bookId
        });
    }

    [RelayCommand]
    private async Task CancelOrExtendReservationAsync(ReservationModel reservation)
    {
        switch (reservation.Status)
        {
            case ReservationStatus.Created:
            case ReservationStatus.CanRetrieve:

                var cancel = await Shell.Current.DisplayAlert("Warning", $"Do you really want to cancel reservation for '{reservation.BookName}'", "Yes", "No");
                if (!cancel)
                {
                    return;
                }
                await apiClient.CancelReservationAsync(reservation.Id);
                await Toast.Make("Reservation was cancelled", ToastDuration.Long).Show();
                await InitializeAsync();
                break;
            case ReservationStatus.Retrieved:
            case ReservationStatus.Extended:
                var extend = await Shell.Current.DisplayAlert("Warning", $"Do you want to extend reservation for '{reservation.BookName}'", "Yes", "No");
                if (!extend)
                {
                    return;
                }
                await apiClient.ExtendReservationAsync(reservation.Id);
                await Toast.Make("Reservation was extended", ToastDuration.Long).Show();
                await InitializeAsync();
                break;
            default:
                break;
        }
    }

    public Task InitializeAsync()
    {
        SelectedStatus = (ReservationStatus)100;
        return LoadReservationsAsync();
    }

    private async Task LoadReservationsAsync(ReservationStatus? status = null)
    {
        var statuses = status is null ? Array.Empty<ReservationStatus>() : new[] { status.Value };
        var result = (await apiClient.GetReservationsAsync(new()
        {
            BookIds = Array.Empty<int>(),
            UserIds = Array.Empty<int>(),
            OrderBy = "",
            IsAscending = true,
            Statuses = statuses,
            Page = 1,
            PageSize = 100
        })).Result.Data;

        Reservations.Clear();
        foreach (var item in result)
        {
            Reservations.Add(item);
        }
    }
}
