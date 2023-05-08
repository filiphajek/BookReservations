namespace BookReservations.Infrastructure;

public static class BookReservationsPolicies
{
    public const string UserPolicy = nameof(UserPolicy);
    public const string LibrarianPolicy = nameof(LibrarianPolicy);
    public const string CanUpdateBookReservationPolicy = nameof(CanUpdateBookReservationPolicy);
    public const string ViewAllUsers = nameof(ViewAllUsers);
    public const string AdminPolicy = nameof(AdminPolicy);
    public const string ProfilePolicy = nameof(ProfilePolicy);
    public const string MsOidc = nameof(MsOidc);
}
