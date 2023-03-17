namespace BookReservations.Api.BL.Models;

public class BookUserInfoModel
{
    public int Id { get; set; }
    public string Isbn { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public bool IsInReservations { get; set; }
    public bool IsInWishlist { get; set; }
}
