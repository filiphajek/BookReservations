using BookReservations.Api.Client;
using BookReservations.App.Views;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
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
        if (!Book.IsAvailable)
        {
            await apiClient.SubscribeToBooksAsync(new[] { Id });
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
        Rating = (float)detail.Result.Reviews.Sum(i => i.Rating) / detail.Result.Reviews.Count;
        UserReview = detail.Result.Reviews.FirstOrDefault(i => i.UserId == userId);
        IsRefreshing = false;
    }
}
