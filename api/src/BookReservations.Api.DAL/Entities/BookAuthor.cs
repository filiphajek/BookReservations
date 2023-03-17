using BookReservations.Infrastructure.DAL;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookReservations.Api.DAL.Entities;

public class BookAuthor : Entity
{
    public int BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    public virtual Book Book { get; set; } = default!;

    public int AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public virtual Author Author { get; set; } = default!;
}