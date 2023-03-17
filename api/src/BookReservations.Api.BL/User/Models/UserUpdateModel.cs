namespace BookReservations.Api.BL.Models;

public class UserUpdateModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Image { get; set; } = default!;
}