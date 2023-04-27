using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class BookViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;

    public BookViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public int Id { get; set; }

    [ObservableProperty]
    private BookDetailModel book = new();

    public async Task InitializeAsync()
    {
        var detail = await apiClient.GetBookDetailAsync(Id);
        Book = detail.Result;
    }
}