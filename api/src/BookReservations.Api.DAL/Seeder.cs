using Bogus;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Enums;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.DAL.EFcore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BookReservations.Api.DAL;

public class Seeder : ISeeder
{
    private readonly IConfiguration configuration;
    private readonly DatabaseOptions databaseOptions;

    public Seeder(IConfiguration configuration, IOptions<DatabaseOptions> databaseOptions)
    {
        this.configuration = configuration;
        this.databaseOptions = databaseOptions.Value;
    }

    public bool CanSeed()
    {
        if (!databaseOptions.Seed)
        {
            return false;
        }

        try
        {
            using var conn = new SqlConnection(configuration.GetConnectionString("Default"));
            conn.Open();
            var reader = new SqlCommand("SELECT TOP 1 1 AS [Exists] FROM Books", conn).ExecuteReader();
            return !reader.Read();
        }
        catch { }

        return true;
    }

    public void Seed(ModelBuilder modelBuilder)
    {
        var f = new Faker();
        Randomizer.Seed = new Random(1338);
        Bogus.DataSets.Date.SystemClock = () => DateTime.Parse("8/8/2017 2:00 PM");
        int globalIndex = 10;

        var generatedBooks = new Faker<Book>()
                .RuleFor(b => b.Name, f => string.Join(" ", f.Lorem.Words(2)))
                .RuleFor(b => b.Isbn, f => f.Random.Replace("###-#-####-####-#"))
                .RuleFor(b => b.AvailableAmount, f => f.Random.Number(0, 5))
                .RuleFor(b => b.Language, f => f.PickRandom(new[] { "Czech", "Slovak", "English", "German", "French", "Spanish" }))
                .RuleFor(b => b.Description, f => f.Lorem.Paragraphs())
                .RuleFor(b => b.Image, f => f.PickRandom(new[]
                {
                    "https://img.freepik.com/free-vector/open-blue-book-white_1308-69339.jpg?w=2000",
                    "https://upload.wikimedia.org/wikipedia/en/thumb/9/99/Question_book-new.svg/2560px-Question_book-new.svg.png",
                    "https://upload.wikimedia.org/wikipedia/commons/b/b6/Gutenberg_Bible%2C_Lenox_Copy%2C_New_York_Public_Library%2C_2009._Pic_01.jpg",
                    "https://img.freepik.com/free-psd/book-cover-mockup_125540-572.jpg?w=2000",
                    "https://smash-images.photobox.com/optimised/258051077d64ddcb431797f201261f68d260b8e4_file_desktop_5760x4512-12ideas-min.jpg",
                    "https://www.foliosociety.com/media/wysiwyg/Howl_Lpod_2.jpg",
                    "https://media.timeout.com/images/103072895/750/562/image.jpg",
                    "https://99designs-blog.imgix.net/blog/wp-content/uploads/2018/01/attachment_73599840-e1516026193959.png?auto=format&q=60&fit=max&w=930",
                    "https://images-platform.99static.com//-TxcMPF6zrslNuDINmoWH5B7I0s=/0x0:1042x1042/fit-in/590x590/99designs-contests-attachments/93/93938/attachment_93938949"
                }))
                .FinishWith((i, j) =>
                {
                    j.TotalAmount = j.AvailableAmount + i.Random.Number(2, 4);
                    j.Id = globalIndex;
                    globalIndex++;
                }).Generate(200);

        globalIndex = 10;

        var generatedAuthors = new Faker<Author>()
            .RuleFor(a => a.FirstName, f => f.Name.FirstName())
            .RuleFor(a => a.LastName, f => f.Name.LastName())
            .RuleFor(a => a.Nationality, f => f.Address.CountryCode())
            .RuleFor(a => a.Birthdate, f => f.Date.Past(18, new DateTime(1900, 1, 1)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(150);

        globalIndex = 10;

        var generatedBookAuthors = new Faker<BookAuthor>()
            .RuleFor(i => i.AuthorId, i => i.PickRandom(generatedAuthors.Select(i => i.Id)))
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(270);

        foreach (var (book, index) in generatedBooks.Select((book, index) => (book, index + 10)))
        {
            var bookWithoutAuthor = generatedBookAuthors.FirstOrDefault(i => book.Id == i.Id);
            if (bookWithoutAuthor is null)
            {
                var randomNumber = f.Random.Number(15, generatedAuthors.Count - 5);
                generatedBookAuthors.Add(new BookAuthor
                {
                    BookId = book.Id,
                    AuthorId = generatedAuthors.First(i => i.Id == randomNumber).Id,
                    Id = generatedBookAuthors.Count + 1,
                });
            }
        }

        globalIndex = 10;

        var generatedUsers = new Faker<User>()
            .RuleFor(i => i.Role, i => BookReservationsRoles.User)
            .RuleFor(i => i.FirstName, i => i.Person.FirstName)
            .RuleFor(i => i.LastName, i => i.Person.LastName)
            .RuleFor(i => i.Email, i => i.Person.Email)
            .RuleFor(i => i.UserName, i => i.Person.UserName)
            .RuleFor(i => i.Image, i => i.Person.Avatar)
            .RuleFor(i => i.Password, i => BCrypt.Net.BCrypt.HashPassword("12345"))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(800).Concat(new Faker<User>()
            .RuleFor(i => i.Role, i => BookReservationsRoles.Librarian)
            .RuleFor(i => i.FirstName, i => i.Person.FirstName)
            .RuleFor(i => i.LastName, i => i.Person.LastName)
            .RuleFor(i => i.Email, i => i.Person.Email)
            .RuleFor(i => i.UserName, i => i.Person.UserName)
            .RuleFor(i => i.Image, i => i.Person.Avatar)
            .RuleFor(i => i.Password, i => BCrypt.Net.BCrypt.HashPassword("12345"))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            }).Generate(8)).ToList();

        globalIndex = 10;

        var generatedReviews = new Faker<Review>()
            .RuleFor(i => i.Rating, i => i.Random.Number(1, 5))
            .RuleFor(i => i.Text, i => i.Lorem.Sentences(i.Random.Number(1, 4)))
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(150)
            .Concat(new Faker<Review>()
            .RuleFor(i => i.Rating, i => i.Random.Number(1, 5))
            .RuleFor(i => i.Text, i => i.Lorem.Sentences(i.Random.Number(1, 4)))
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .RuleFor(i => i.UserId, i => i.PickRandom(generatedUsers.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(350));

        globalIndex = 10;

        var generatedReservations = new Faker<Reservation>()
            .RuleFor(i => i.Status, i => i.PickRandom(new[] { ReservationStatus.Extended, ReservationStatus.Created, ReservationStatus.Retrieved }))
            .RuleFor(i => i.From, i => i.Date.Recent(5, new DateTime(2021, 1, 1)))
            .RuleFor(i => i.To, i => i.Date.Soon(5, new DateTime(2022, 12, 30)))
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .RuleFor(i => i.UserId, i => i.PickRandom(generatedUsers.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(400)
            .Concat(new Faker<Reservation>()
            .RuleFor(i => i.Status, i => ReservationStatus.Cancelled)
            .RuleFor(i => i.From, i => i.Date.Past(1, new DateTime(2020, 1, 1)))
            .RuleFor(i => i.To, i => DateTime.MinValue)
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .RuleFor(i => i.UserId, i => i.PickRandom(generatedUsers.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(100))
            .Concat(new Faker<Reservation>()
            .RuleFor(i => i.Status, i => ReservationStatus.Returned)
            .RuleFor(i => i.From, i => i.Date.Past(1, new DateTime(2021, 1, 1)))
            .RuleFor(i => i.To, i => i.Date.Recent(5, new DateTime(2021, 1, 1)))
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .RuleFor(i => i.UserId, i => i.PickRandom(generatedUsers.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(800));

        globalIndex = 10;

        var userBookRelationsReservations = new Faker<UserBookRelations>()
            .RuleFor(i => i.RelationType, i => (UserBookRelationType)i.Random.Number(0, 1))
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .RuleFor(i => i.UserId, i => i.PickRandom(generatedUsers.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(2000);

        generatedUsers.AddRange(new[]
        {
            new User
            {
                Email = "admin@booksres.com",
                FirstName = "Tomas",
                LastName = "Dvorak",
                Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                UserName = "admin",
                Id = 8,
                Role = BookReservationsRoles.Admin
            },
            new User
            {
                Email = "michal-librarian@booksres.com",
                FirstName = "Michal",
                LastName = "Novak",
                Password = BCrypt.Net.BCrypt.HashPassword("12345"),
                UserName = "michal6",
                Id = 9,
                Role = BookReservationsRoles.Librarian
            }
        });

        modelBuilder.Entity<Book>().HasData(generatedBooks);
        modelBuilder.Entity<Author>().HasData(generatedAuthors);
        modelBuilder.Entity<BookAuthor>().HasData(generatedBookAuthors);
        modelBuilder.Entity<Review>().HasData(generatedReviews);
        modelBuilder.Entity<User>().HasData(generatedUsers);
        modelBuilder.Entity<Reservation>().HasData(generatedReservations);
        modelBuilder.Entity<UserBookRelations>().HasData(userBookRelationsReservations);
    }
}