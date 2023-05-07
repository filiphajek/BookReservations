using BookReservations.Api.Client;
using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class WishlistPage
{
    public WishlistPage(RelationsViewModel vm) : base(vm)
    {
        InitializeComponent();
        vm.RelationType = UserBookRelationType.Wishlist;
    }
}