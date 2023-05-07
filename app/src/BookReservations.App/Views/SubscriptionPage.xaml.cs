using BookReservations.Api.Client;
using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class SubscriptionPage
{
    public SubscriptionPage(RelationsViewModel vm) : base(vm)
    {
        InitializeComponent();
        vm.RelationType = UserBookRelationType.Subscription;
    }
}