using BookReservations.Api.DAL.Enums;

namespace BookReservations.Api.BL.Models;

public class RelationInfoModel
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookName { get; set; } = string.Empty;
    public UserBookRelationType RelationType { get; set; }
}
