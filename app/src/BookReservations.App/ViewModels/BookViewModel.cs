using BookReservations.Api.Client;
using BookReservations.App.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IdentityModel.Tokens.Jwt;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class BookViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;
    private readonly ISecureStorage storage;

    public BookViewModel(IApiClient apiClient, ISecureStorage storage)
    {
        this.apiClient = apiClient;
        this.storage = storage;
    }

    public int Id { get; set; }

    [ObservableProperty]
    private bool isRefreshing = true;

    [ObservableProperty]
    private float rating = 0;

    [ObservableProperty]
    private ReviewModel userReview = null;

    [ObservableProperty]
    private BookDetailModel book = new();

    [ObservableProperty]
    private BookUserInfoModel bookUserModel;
    private int userId = 0;

    [RelayCommand]
    private Task RefreshAsync() => InitializeAsync();

    [RelayCommand]
    private async Task GoToReviews()
    {
        await Shell.Current.GoToAsync("reviews", new Dictionary<string, object>
        {
            [nameof(Id)] = Id
        });
    }

    [RelayCommand]
    private async Task AddToWishlistAsync()
    {
        if (BookUserModel.IsInWishlist)
        {
            return;
        }
        await apiClient.AddBooksToWishlistAsync(new[] { Id });
        BookUserModel.IsInWishlist = true;
        var toast = Toast.Make("Added to wishlist", ToastDuration.Long);
        await toast.Show();
    }

    [RelayCommand]
    private async Task GoToUserReviewAsync()
    {
        var review = UserReview ?? new() { BookId = Id, UserId = userId };
        await Shell.Current.GoToAsync("add", new Dictionary<string, object>
        {
            [nameof(UserReviewViewModel.BookId)] = Id,
            [nameof(UserReviewViewModel.Review)] = review,
        });
    }

    [RelayCommand]
    private async Task ReservateOrSubscribeAsync()
    {
        if (BookUserModel.IsInReservations)
        {
            return;
        }
        if (!Book.IsAvailable)
        {
            try
            {
                await apiClient.SubscribeToBooksAsync(new[] { Id });
                await Toast.Make("Subscribed", ToastDuration.Long).Show();
            }
            catch (SwaggerException ex)
            {
                var err = ex.StatusCode == 204 ? "Already subscribed" : "Something went wrong";
                await Toast.Make(err, ToastDuration.Long).Show();
            }
            return;
        }
        var popup = new AddReservationPopup();
        if (await Shell.Current.CurrentPage.ShowPopupAsync(popup) is not null)
        {
            var vw = popup.BindingContext as AddReservationViewModel;
            var result = await apiClient.MakeReservationAsync(new()
            {
                From = vw.FromDate,
                To = vw.ToDate,
                BookId = Book.Id,
            });
            if (result.StatusCode != 200)
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong, try again", "Ok");
                return;
            }
            await Shell.Current.CurrentPage.DisplayAlert("Success", "You created a reservation", "Ok");
            await InitializeAsync();
        }
    }

    public async Task InitializeAsync()
    {
        IsRefreshing = true;

        var token = await storage.GetAsync("token");
        var claims = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims;
        userId = int.Parse(claims.FirstOrDefault(x => x.Type == "book-res-userid")?.Value ?? string.Empty);

        BookUserModel = (await apiClient.GetUserBooksAsync(Id)).Result;
        var detail = await apiClient.GetBookDetailAsync(Id);
        Book = detail.Result;
        Book.IsAvailable = (await apiClient.IsBookAvailableAsync(Id)).StatusCode == 200;
        Rating = detail.Result.Reviews.Count == 0 ? 0 : (float)detail.Result.Reviews.Sum(i => i.Rating) / detail.Result.Reviews.Count;
        UserReview = detail.Result.Reviews.FirstOrDefault(i => i.UserId == userId);
        IsRefreshing = false;
    }
}
