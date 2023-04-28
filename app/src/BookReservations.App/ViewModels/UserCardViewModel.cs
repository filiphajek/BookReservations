using BookReservations.App.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookReservations.App.ViewModels;

public partial class UserCardViewModel : ObservableObject, IViewModel
{
    private readonly ISecureStorage _storage;

    public UserCardViewModel(ISecureStorage storage)
    {
        _storage = storage;
    }

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    private int id = 0;

    [RelayCommand]
    public async Task GoToUserDetailAsync()
    {
        await Shell.Current.GoToAsync(UserDetailPage.Route, new Dictionary<string, object>
        {
            [nameof(UserDetailViewModel.Id)] = id
        });
    }

    public async Task InitializeAsync()
    {
        var token = await _storage.GetAsync("token");
        var claims = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims;
        Username = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        Email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty;
        id = int.Parse(claims.FirstOrDefault(x => x.Type == "book-res-userid")?.Value ?? string.Empty);
    }
}
