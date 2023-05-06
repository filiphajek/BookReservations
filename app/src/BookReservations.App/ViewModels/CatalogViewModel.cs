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
        PropertyChanged += FilterPropertyChanged;
    }

    private async void FilterPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (IsRefreshing)
        {
            await Task.Delay(400);
            return;
        }

        if (e.PropertyName == nameof(Ascending) || e.PropertyName == nameof(IsAvailable) || e.PropertyName == nameof(SearchText))
        {
            await RefreshAsync();
        }
    }

    [ObservableProperty]
    private bool isRefreshing = true;

    [ObservableProperty]
    private bool ascending = true;

    [ObservableProperty]
    private bool isAvailable = true;

    [ObservableProperty]
    private string searchText = "";

    [ObservableProperty]
    private ObservableCollection<BookModel> books = new();

    private int page = 1;
    private bool hasAllPages = false;

    public Task InitializeAsync() => GetBookAsync();

    [RelayCommand]
    private async Task LoadNextPageAsync()
    {
        page++;
        await GetBookAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        page = 1;
        hasAllPages = false;
        Books.Clear();
        await InitializeAsync();
    }

    [RelayCommand]
    private async Task GoToDetail(int id)
    {
        await Shell.Current.GoToAsync("detail", new Dictionary<string, object>
        {
            [nameof(BookViewModel.Id)] = id
        });
    }

    private async Task GetBookAsync()
    {
        if (hasAllPages)
        {
            return;
        }

        IsRefreshing = true;
        var response = await apiClient.GetBooksAsync(new GetBooksContract
        {
            Page = page,
            PageSize = 20,
            IsAscending = Ascending,
            OnlyAvailable = IsAvailable,
            SearchText = SearchText,
            OrderBy = nameof(BookModel.Name),
        });

        if (response.Result.TotalCount == Books.Count)
        {
            hasAllPages = true;
        }

        var existingids = Books.Select(i => i.Id).ToArray();
        foreach (var item in response.Result.Data.Where(i => !existingids.Contains(i.Id)))
        {
            Books.Add(item);
        }
        IsRefreshing = false;
    }
}
