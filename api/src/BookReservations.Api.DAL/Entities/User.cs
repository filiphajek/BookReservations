using BookReservations.Infrastructure.DAL;
using System.ComponentModel.DataAnnotations;

namespace BookReservations.Api.DAL.Entities;

public class User : Entity, IUserIdProtection
{
    [MaxLength(32)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(32)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(32)]
    public string UserName { get; set; } = string.Empty;

    [MaxLength(128)]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    [MaxLength(32)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(128)]
    public string? MsId { get; set; } = string.Empty;

    public virtual ICollection<UserBookRelations> Relations { get; set; } = new List<UserBookRelations>();
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public int UserId => Id;
}
