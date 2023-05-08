using BookReservations.Api.Client;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookReservations.App.ViewModels;


public partial class RelationsViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;

    [ObservableProperty]
    private ObservableCollection<RelationInfoModel> relationInfos = new();

    private readonly List<int> selectedRelationIds = new();

    public UserBookRelationType RelationType { get; set; }

    public RelationsViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    [RelayCommand]
    private async Task RemoveRelationAsync()
    {
        var ids = selectedRelationIds.Distinct().ToArray();
        if (!ids.Any())
        {
            return;
        }
        var remove = await Shell.Current.DisplayAlert("Warning", "Are you sure?", "Yes", "No");
        if (remove)
        {
            await apiClient.DeleteRelationsAsync(ids);
            await Toast.Make("Relations was removed", ToastDuration.Long).Show();
            await InitializeAsync();
        }
    }

    [RelayCommand]
    private async Task GoToBookDetailAsync(int bookId)
    {
        await Shell.Current.GoToAsync("//books/detail", new Dictionary<string, object>()
        {
            [nameof(BookViewModel.Id)] = bookId
        });
    }

    [RelayCommand]
    private void SelectRelation(int bookId)
    {
        if (selectedRelationIds.Contains(bookId))
        {
            selectedRelationIds.Remove(bookId);
            return;
        }
        selectedRelationIds.Add(bookId);
    }

    public async Task InitializeAsync()
    {
        RelationInfos = new();
        selectedRelationIds.Clear();
        var relations = (await apiClient.GetRelationsAsync(RelationType)).Result;
        foreach (var relation in relations)
        {
            RelationInfos.Add(relation);
        }
    }
}
