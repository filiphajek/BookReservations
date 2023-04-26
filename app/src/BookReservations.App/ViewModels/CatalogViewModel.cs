using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;
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
}
