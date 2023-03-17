using BookReservations.Infrastructure.DAL;
using System.ComponentModel.DataAnnotations;

namespace BookReservations.Api.DAL.Entities;

public class Author : Entity
{
    [MaxLength(32)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(32)]
    public string LastName { get; set; } = string.Empty;

    public ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    [MaxLength(32)]
    public string Nationality { get; set; } = string.Empty;

    public DateTime Birthdate { get; set; }
}
