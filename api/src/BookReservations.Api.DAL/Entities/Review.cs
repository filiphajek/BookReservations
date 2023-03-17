using BookReservations.Infrastructure.DAL;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReservations.Api.DAL.Entities;

public class Review : Entity
{
    public int? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; } = default!;

    public int BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    public virtual Book Book { get; set; } = default!;

    public int Rating { get; set; }

    [MaxLength(2048)]
    public string Text { get; set; } = string.Empty;
}