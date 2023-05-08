namespace BookReservations.Api.BL.Models;

public class UserInfoModel
{
    public string UserName { get; set; } = default!;
    public int Id { get; set; }
    public string Role { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Image { get; set; } = default!;
    public bool IsMsOidc { get; set; } = false;
}