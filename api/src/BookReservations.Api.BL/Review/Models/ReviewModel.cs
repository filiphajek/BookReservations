namespace BookReservations.Api.BL.Models;

public class ReviewModel
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int BookId { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
}
