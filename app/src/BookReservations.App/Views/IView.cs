using BookReservations.App.ViewModels;

namespace BookReservations.App.Views;

public interface IView
{
    public IViewModel ViewModel { get; }
}
