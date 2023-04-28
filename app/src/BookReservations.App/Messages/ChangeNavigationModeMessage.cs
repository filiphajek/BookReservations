namespace BookReservations.App.Messages;

public enum NavigationMode
{
    Shell,
    NavPage
}

public record ChangeNavigationModeMessage(NavigationMode Mode);
