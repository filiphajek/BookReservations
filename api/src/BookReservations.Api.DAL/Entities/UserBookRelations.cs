using BookReservations.Api.DAL.Enums;
using BookReservations.Infrastructure.DAL;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReservations.Api.DAL.Entities;

public class UserBookRelations : Entity, IUserIdProtection
{
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = default!;

    public int BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    public virtual Book Book { get; set; } = default!;

    public UserBookRelationType RelationType { get; set; }
}

