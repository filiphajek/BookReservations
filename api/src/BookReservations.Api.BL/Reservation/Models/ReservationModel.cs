using BookReservations.Api.DAL.Enums;

namespace BookReservations.Api.BL.Models;

public class ReservationModel
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookName { get; set; } = string.Empty;
    public int TotalAmount { get; set; }
    public int AvailableAmount { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public ReservationStatus Status { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
}
