using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookReservations.App.ViewModels;

public partial class CatalogViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;

    public CatalogViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    [ObservableProperty]
    private ObservableCollection<BookModel> books = new();

    public async Task InitializeAsync()
    {
        var response = await apiClient.GetBooksAsync(new GetBooksContract
        {
            Page = 1,
            PageSize = 20
        });
        Books = new(response.Result.Data);
    }

    [RelayCommand]
    private async Task GoToDetail(int id)
    {
        await Shell.Current.GoToAsync("detail", new Dictionary<string, object>
        {
            [nameof(BookViewModel.Id)] = id
        });
    }
}
