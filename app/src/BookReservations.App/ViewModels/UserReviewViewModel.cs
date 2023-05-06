using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(BookId), nameof(BookId))]
[QueryProperty(nameof(Review), nameof(Review))]
public partial class UserReviewViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;

    public UserReviewViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public int BookId { get; set; }

    [ObservableProperty]
    private ReviewModel review;

    [RelayCommand]
    private async Task SaveReviewAsync()
    {
        await apiClient.AddReviewAsync(Review);
    }

    [RelayCommand]
    private async Task DeleteReviewAsync()
    {
        if (Review is null || Review.Id == 0)
        {
            return;
        }
        await apiClient.DeleteReviewAsync(Review.Id);
    }

    public Task InitializeAsync() => Task.CompletedTask;
}