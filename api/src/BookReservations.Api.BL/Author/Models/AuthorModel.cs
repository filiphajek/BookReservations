namespace BookReservations.Api.BL.Models;

public class AuthorModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
}
