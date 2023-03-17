namespace BookReservations.Api.BL.Models;

public class BookModel
{
    public int Id { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public int TotalAmount { get; set; }
    public int AvailableAmount { get; set; }
    public bool IsAvailable { get; set; }
}
