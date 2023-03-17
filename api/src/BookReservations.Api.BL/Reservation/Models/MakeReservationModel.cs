namespace BookReservations.Api.BL.Models;

public class MakeReservationModel
{
    public int BookId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}