using BookReservations.Api.Client;
using BookReservations.App.Messages;
using BookReservations.App.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class UserDetailViewModel : ObservableObject, IViewModel
{
    private readonly static FilePickerFileType fileType = new(new Dictionary<DevicePlatform, IEnumerable<string>>()
    {
        { DevicePlatform.Android, new[] { "image/jpeg", "image/png" } },
    });

    public int Id { get; set; }

    private readonly IApiClient apiClient;
    private readonly IMessengerService messengerService;
    private FileResult fileResult;

    [ObservableProperty]
    private UserInfoModel user;

    [ObservableProperty]
    private ImageSource image;

    [ObservableProperty]
    private string error;

    public UserDetailViewModel(IApiClient apiClient, IMessengerService messengerService)
    {
        this.apiClient = apiClient;
        this.messengerService = messengerService;
    }

    [RelayCommand]
    private async Task SelectImageAsync()
    {
        fileResult = await FilePicker.Default.PickAsync(new()
        {
            PickerTitle = "Pick your profile image",
            FileTypes = fileType
        });
        if (fileResult is not null)
        {
            var imageStream = await fileResult.OpenReadAsync();
            Image = ImageSource.FromStream(() => imageStream);
        }
    }

    [RelayCommand]
    private async Task SaveProfileAsync()
    {
        var fileParam = fileResult is not null ? new FileParameter(await fileResult.OpenReadAsync(), fileResult.FileName, fileResult.ContentType) : null;
        var imageName = fileResult is not null ? fileResult.FileName : User.Image;
        try
        {
            Error = "";
            await apiClient.UpdateUserAsync(Id, User.UserName, User.Email, User.FirstName, User.LastName, imageName, fileParam);

            var userInfo = await apiClient.GetUserInfoAsync();
            messengerService.Send(new UserProfileChanged(userInfo.Result));
            var toast = Toast.Make("Profile was saved", ToastDuration.Long);
            await toast.Show();
        }
        catch (Exception ex)
        {
            Error = ex switch
            {
                SwaggerException<UpdateUserResponse> err => string.Join(Environment.NewLine, err.Result.Errors.First().Value),
                SwaggerException<ValidationErrorResponse> err => string.Join(Environment.NewLine, err.Result.Errors.First().Value),
                _ => "Something went wrong, try again"
            };
            var toast = Toast.Make(Error, ToastDuration.Long);
            await toast.Show();
        }
    }

    public async Task InitializeAsync()
    {
        User = (await apiClient.GetUserInfoAsync()).Result;
    }
}
