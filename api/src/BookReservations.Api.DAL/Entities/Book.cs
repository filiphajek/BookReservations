using BookReservations.Infrastructure.DAL;
using System.ComponentModel.DataAnnotations;

namespace BookReservations.Api.DAL.Entities;

public class Book : Entity
{
    [MaxLength(32)]
    [MinLength(10)]
    public string Isbn { get; set; } = string.Empty;

    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string Image { get; set; } = string.Empty;

    [MaxLength(1024)] 
    public string Language { get; set; } = string.Empty;
    
    public int TotalAmount { get; set; }

    public int AvailableAmount { get; set; }

    public ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
