using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.DAL.EFcore;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.DAL;

#nullable disable warnings

public class BookReservationsDbContext : DbContext
{
    private readonly ISeeder seeder;

    public BookReservationsDbContext(
        DbContextOptions<BookReservationsDbContext> options,
        ISeeder seeder) : base(options)
    {
        this.seeder = seeder;
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<UserBookRelations> Relations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (seeder.CanSeed())
        {
            seeder.Seed(modelBuilder);
        }

        modelBuilder.Entity<Author>()
            .HasMany(p => p.Books)
            .WithMany(p => p.Authors)
            .UsingEntity<BookAuthor>(
                j => j
                    .HasOne(pt => pt.Book)
                    .WithMany(t => t.BookAuthors)
                    .HasForeignKey(pt => pt.BookId)
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne(pt => pt.Author)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(pt => pt.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade));

        modelBuilder.Entity<User>()
            .HasMany(i => i.Relations)
            .WithOne(i => i.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Review>()
           .HasOne(i => i.User)
           .WithMany()
           .HasForeignKey(p => p.UserId)
           .OnDelete(DeleteBehavior.SetNull);

        base.OnModelCreating(modelBuilder);
    }
}

#nullable enable warnings
