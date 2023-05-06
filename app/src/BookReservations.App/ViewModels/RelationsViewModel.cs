using BookReservations.Api.Client;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BookReservations.App.ViewModels;

[QueryProperty(nameof(RelationType), nameof(RelationType))]
public partial class RelationsViewModel : ObservableObject, IViewModel
{
    private readonly IApiClient apiClient;

    [ObservableProperty]
    private ObservableCollection<RelationInfoModel> relationInfos = new();

    public UserBookRelationType RelationType { get; set; }

    public RelationsViewModel(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public async Task InitializeAsync()
    {
        var relations = (await apiClient.GetRelationsAsync(RelationType)).Result;
        foreach (var relation in relations)
        {
            RelationInfos.Add(relation);
        }
    }
}
