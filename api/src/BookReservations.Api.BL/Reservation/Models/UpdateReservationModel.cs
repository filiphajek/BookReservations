using BookReservations.Api.DAL.Enums;

namespace BookReservations.Api.BL.Models;

public class UpdateReservationModel
{
    public int Id { get; set; }
    public ReservationStatus Status { get; set; }
}