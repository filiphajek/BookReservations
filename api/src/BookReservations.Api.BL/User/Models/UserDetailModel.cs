namespace BookReservations.Api.BL.Models;

public class UserDetailModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Image { get; set; } = default!;
    public string Role { get; set; } = default!;
    public ICollection<ReservationModel> Reservations { get; set; } = new List<ReservationModel>();
    public ICollection<RelationInfoModel> Relations { get; set; } = new List<RelationInfoModel>();
}
