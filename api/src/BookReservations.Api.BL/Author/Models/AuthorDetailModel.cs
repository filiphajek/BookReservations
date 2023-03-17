namespace BookReservations.Api.BL.Models;

public class AuthorDetailModel
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Image { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public DateTime Birthdate { get; set; }
    public ICollection<BookModel> Books { get; set; } = new List<BookModel>();
}