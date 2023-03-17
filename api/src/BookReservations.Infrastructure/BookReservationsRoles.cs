namespace BookReservations.Infrastructure;

public static class BookReservationsRoles
{
    public const string User = nameof(User);
    public const string Librarian = nameof(Librarian);
    public const string Admin = nameof(Admin);
    public static string[] AllRoles = new[] { User.ToLower(), Librarian.ToLower(), Admin.ToLower() };
}
