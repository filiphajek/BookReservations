using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookReservations.App.ViewModels;

public partial class ReservationViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;

    public ReservationViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    [ObservableProperty]
    private ObservableCollection<ReservationModel> reservations = new();

    [RelayCommand]
    private async Task GoToBookDetailAsync(int bookId)
    {
        await Shell.Current.GoToAsync("//book/detail", new Dictionary<string, object>()
        {
            [nameof(BookViewModel.Id)] = bookId
        });
    }

    public async Task InitializeAsync()
    {
        var result = (await apiClient.GetReservationsAsync(new()
        {
            BookIds = Array.Empty<int>(),
            UserIds = Array.Empty<int>(),
            OrderBy = "",
            IsAscending = true,
            Statuses = Array.Empty<ReservationStatus>(),
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
