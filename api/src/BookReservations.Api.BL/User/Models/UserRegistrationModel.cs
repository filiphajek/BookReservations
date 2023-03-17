namespace BookReservations.Api.BL.Models;

public class UserRegistrationModel
{
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Image { get; set; } = default!;
    public string Password { get; set; } = default!;
}
