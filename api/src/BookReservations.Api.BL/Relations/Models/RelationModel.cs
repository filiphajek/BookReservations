using BookReservations.Api.DAL.Enums;

namespace BookReservations.Api.BL.Models;

public class RelationModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public UserBookRelationType RelationType { get; set; }
}
