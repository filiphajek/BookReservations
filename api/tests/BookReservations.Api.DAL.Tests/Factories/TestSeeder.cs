using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Enums;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.DAL.EFcore;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.DAL.Tests.Factories;

public class TestSeeder : ISeeder
{
    public bool CanSeed()
    {
        return true;
    }

    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Soukup",
                Birthdate = DateTime.Today,
                Nationality = "Czech"
            });

        modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 2,
                FirstName = "Jiri",
                LastName = "Soukup",
                Birthdate = DateTime.Today - TimeSpan.FromDays(1),
                Nationality = "Czech"
            });

        modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 3,
                FirstName = "Karl",
                LastName = "Marx",
                Birthdate = DateTime.MinValue,
                Nationality = "German"
            });

        modelBuilder.Entity<Book>().HasData(
            new Book()
            {
                Id = 1,
                Isbn = "978-0001112222",
                Name = "One rule for life",
                Description = "A nonexistent advice",
                AvailableAmount = 10,
                Language = "English"
            });

        modelBuilder.Entity<Book>().HasData(
            new Book()
            {
                Id = 2,
                Isbn = "978-3868205961",
                Name = "Das Kapital",
                Description = "Critical viewpoint of political economics",
                AvailableAmount = 200,
                Language = "German"
            });

        modelBuilder.Entity<BookAuthor>().HasData(
            new BookAuthor()
            {
                Id = 1,
                BookId = 1,
                AuthorId = 1,
            });

        modelBuilder.Entity<BookAuthor>().HasData(
            new BookAuthor()
            {
                Id = 2,
                BookId = 2,
                AuthorId = 1,
            });

        modelBuilder.Entity<BookAuthor>().HasData(
            new BookAuthor()
            {
                Id = 3,
                BookId = 1,
                AuthorId = 2,
            });

        modelBuilder.Entity<BookAuthor>().HasData(
            new BookAuthor()
            {
                Id = 4,
                BookId = 1,
                AuthorId = 3,
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                UserName = "xbrlej",
                FirstName = "Simon",
                LastName = "Brlej",
                Password = "whatever123",
                Email = "123456@gmail.com",
                Role = BookReservationsRoles.Librarian
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 2,
                UserName = "xhajek",
                FirstName = "Filip",
                LastName = "Hajek",
                Password = "whatever456",
                Email = "987654@seznam.cz",
                Role = BookReservationsRoles.Librarian
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 3,
                UserName = "pnovak",
                FirstName = "Petr",
                LastName = "Novak",
                Password = "NovPetr4181",
                Email = "petrnovak@hotmail.co.uk",
                Role = BookReservationsRoles.User
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 4,
                UserName = "user",
                FirstName = "Felix",
                LastName = "Gutenberg",
                Password = "FelixGut21*",
                Email = "felgut@gmail.com",
                Role = BookReservationsRoles.User
            });

        modelBuilder.Entity<Reservation>().HasData(
            new Reservation
            {
                Id = 1,
                BookId = 1,
                UserId = 3,
                From = DateTime.Today - TimeSpan.FromDays(30),
                To = DateTime.Today,
                Status = ReservationStatus.CanRetrieve
            });

        modelBuilder.Entity<Reservation>().HasData(
            new Reservation
            {
                Id = 2,
                BookId = 2,
                UserId = 4,
                From = DateTime.Today - TimeSpan.FromDays(120),
                To = DateTime.Today - TimeSpan.FromDays(30),
                Status = ReservationStatus.Retrieved
            });

        modelBuilder.Entity<UserBookRelations>().HasData(
            new UserBookRelations
            {
                Id = 1,
                BookId = 1,
                UserId = 4,
                RelationType = UserBookRelationType.Subscription
            });

        modelBuilder.Entity<UserBookRelations>().HasData(
            new UserBookRelations
            {
                Id = 2,
                BookId = 2,
                UserId = 3,
                RelationType = UserBookRelationType.Wishlist
            });

        modelBuilder.Entity<Review>().HasData(
            new Review
            {
                Id = 1,
                BookId = 2,
                UserId = 4,
                Rating = 10,
                Text = "Agreed, communism was better."
            });
    }
}