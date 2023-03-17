using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.Tests.Factories;

namespace BookReservations.Api.DAL.Tests.Fixtures;

public class BookReservationsQueryFixture : BookReservationsDbContextFixture
{
    public BookReservationsQueryFixture(IDbContextFactory<BookReservationsDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    protected override async Task SeedAsync()
    {
        await SeedAuthors();
        await SeedBooks();
        await SeedBookAuthors();
    }

    private async Task SeedAuthors()
    {
        var firstNames = new string[] { "Adam", "Joseph", "Jonas", "Ursula", "Jane", "Thomas", "Stephen" };
        var lastNames = new string[] { "Morton", "Hughes", "Jelinek", "Norman", "Thorne", "Norman", "Hoffman" };
        var birthDates = new DateTime[]
        {
            DateTime.Today - TimeSpan.FromDays(10000),
            DateTime.Today - TimeSpan.FromDays(10000),
            DateTime.Today - TimeSpan.FromDays(5000),
            DateTime.Today - TimeSpan.FromDays(5000),
            DateTime.Today - TimeSpan.FromDays(7500),
            DateTime.Today - TimeSpan.FromDays(7500),
            DateTime.Today - TimeSpan.FromDays(3000)
        };
        var nationalities = new string[] { "English", "English", "Czech", "German", "English", "German", "Swedish" };

        for (var i = 0; i < firstNames.Length; i++)
        {
            await DbContext.AddAsync(new Author
            {
                FirstName = firstNames[i],
                LastName = lastNames[i],
                Birthdate = birthDates[i],
                Nationality = nationalities[i]
            });
        }

        await DbContext.SaveChangesAsync();
    }

    private async Task SeedBooks()
    {
        var isbns = new string[]
        {
            "978-0000011111",
            "978-0000022222",
            "978-0000033333",
            "978-0000044444",
            "978-0000055555",
            "978-0000066666"
        };
        var names = new string[]
        {
            "queryTestBookName1",
            "queryTestBookName2",
            "queryTestBookName3",
            "queryTestBookName4",
            "queryTestBookName5",
            "queryTestBookName6"
        };

        var descriptions = new string[]
        {
            "noDesc",
            "noDesc",
            "someDesc",
            "someDesc",
            "longDescription",
            "longDescription"
        };
        var amounts = new int[]
        {
            50,
            60,
            70,
            100,
            100,
            2000
        };

        var languages = new string[]
        {
            "English",
            "English",
            "German",
            "German",
            "Czech",
            "Czech"
        };

        for (var i = 0; i < isbns.Length; i++)
        {
            await DbContext.AddAsync(new Book
            {
                Isbn = isbns[i],
                Name = names[i],
                Description = descriptions[i],
                AvailableAmount = amounts[i],
                Language = languages[i]
            });
        }

        await DbContext.SaveChangesAsync();
    }

    private async Task SeedBookAuthors()
    {
        var bookIds = new int[] { 3, 3, 3, 4, 5, 5, 5, 5, 5, 6, 6, 8, 8, 8 };
        var authorIds = new int[] { 4, 5, 8, 6, 4, 5, 7, 8, 9, 7, 9, 4, 5, 8 };

        for (var i = 0; i < bookIds.Length; i++)
        {
            await DbContext.AddAsync(new BookAuthor
            {
                BookId = bookIds[i],
                AuthorId = authorIds[i]
            });
        }

        await DbContext.SaveChangesAsync();
    }
}
