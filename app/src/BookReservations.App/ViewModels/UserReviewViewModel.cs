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
        try
        {
            await apiClient.AddReviewAsync(Review);
        }
        catch (SwaggerException ex)
        {
            if (ex.StatusCode == 204)
            {
                await Shell.Current.DisplayAlert("Error", "You can not add review, first, read the book", "Ok");
                return;
            }
            await Shell.Current.DisplayAlert("Error", "Something went wrong, try again", "Ok");
        }
        finally
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    [RelayCommand]
    private async Task DeleteReviewAsync()
    {
        if (Review is not null && Review.Id != 0)
        {
            await apiClient.DeleteReviewAsync(Review.Id);
        }
        await apiClient.DeleteReviewAsync(Review.Id);
        await Shell.Current.GoToAsync("..");
    }

    public Task InitializeAsync() => Task.CompletedTask;
}