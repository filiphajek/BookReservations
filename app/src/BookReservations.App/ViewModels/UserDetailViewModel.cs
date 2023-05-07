using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class UserDetailViewModel : ObservableObject, IViewModel
{
    private readonly static FilePickerFileType fileType = new(new Dictionary<DevicePlatform, IEnumerable<string>>()
    {
        { DevicePlatform.Android, new[] { "image/jpeg", "image/png" } },
        //FilePickerFileType.Images
    });

    public int Id { get; set; }

    private readonly IApiClient apiClient;
    private FileResult fileResult;

    [ObservableProperty]
    private UserInfoModel user;

    public UserDetailViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    [RelayCommand]
    private async Task SelectImageAsync()
    {
        fileResult = await FilePicker.Default.PickAsync(new()
        {
            PickerTitle = "Pick your profile image",
            FileTypes = fileType
        });
    }

    [RelayCommand]
    private async Task SaveProfileAsync()
    {
        if (fileResult is not null)
        {
            using var stream = await fileResult.OpenReadAsync();
            await apiClient.UpdateUserAsync(Id, User.UserName, User.Email, User.FirstName, User.LastName, "", new FileParameter(stream));
            return;
        }
        await apiClient.UpdateUserAsync(Id, User.UserName, User.Email, User.FirstName, User.LastName, "", null);
    }

    public async Task InitializeAsync()
    {
        User = (await apiClient.GetUserInfoAsync()).Result;
    }
}
