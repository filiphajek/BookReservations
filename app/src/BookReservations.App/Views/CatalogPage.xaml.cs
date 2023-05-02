using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public partial class CatalogPage
{
    public const string Route = "catalog";

    public CatalogPage(CatalogViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }

    private void CollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
        {
            return;
        }

        //NOTE: workaround on windows to fire collectionview itemthresholdreached command, because it does not work on windows
        if (sender is CollectionView cv && cv is IElementController element)
        {
            var count = element.LogicalChildren.Count;
            if (e.LastVisibleItemIndex + 1 - count + cv.RemainingItemsThreshold >= 0)
            {
                if (cv.RemainingItemsThresholdReachedCommand.CanExecute(null))
                {
                    cv.RemainingItemsThresholdReachedCommand.Execute(null);
                }
            }
        }
    }
}