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
        Randomizer.Seed = new Random(1825);
        Bogus.DataSets.Date.SystemClock = () => DateTime.Parse("8/8/2017 2:00 PM");
        int globalIndex = 10;

        var fakeBookNames = new[]
        {
            "The Lost City",
            "The Silent Conspiracy",
            "Echoes in the Wind",
            "Thieves of the Sky",
            "The Poisoned Legacy",
            "Shadows of the Dark",
            "The Secret of the Lost Treasure",
            "The Last Chance for Redemption",
            "The Kingdom of Ice and Fire",
            "Whispers from the Abyss",
            "Through the Winding Forest",
            "The Cursed Island",
            "The Phantom Thief's Heist",
            "The Tides of Fate",
            "The Alchemist's Secret",
            "The Forgotten Kingdom",
            "The Shadow of Death",
            "The Lost Oasis",
            "The Crystal Cavern",
            "The Broken Blade",
            "The Secret of the Sleeping Sphinx",
            "The Ghosts of Nightfall Manor",
            "The Dreaded Curse of the Werewolf",
            "The Enchanted Garden",
            "The Sands of Time",
            "The Chosen One",
            "The Rise of the Dragon Queen",
            "The Tales of a Wandering Bard",
            "The Dark Knight's Revenge",
            "The Last Hope for Humanity",
            "The Escape from Alcatraz",
            "The Haunting of Hollow Hills",
            "The Legend of the Golden Fleece",
            "The Age of Magic",
            "The Battle for the Sunken City",
            "The Prophecy of the Red Moon",
            "The Hunters of the Lost Ark",
            "The Keeper of the Crystal Kingdom",
            "The Secret Society of Alchemists",
            "The Voyage of the Pirate Queen",
            "The Curse of the Black Pearl",
            "The Secret of the Sunken Temple",
            "The Tomb of the Sphinx Queen",
            "The Silence of the Lambs",
            "The Secret of the Abandoned Asylum",
            "The Scandalous Tale of the Duchess",
            "The Case of the Missing Heirloom",
            "The Mystery of the Lost Locket",
            "The Shadowy Depths of the Underworld",
            "The Rise of the Dark Lord",
            "The Keeper of the Forbidden Library",
            "The Cursed Diamond",
        };

        var fakeBookDescriptions = new[]
        {
            "A thrilling adventure to uncover the secrets of a lost city.",
            "A top-secret mission to uncover the truth about a hidden conspiracy.",
            "A journey through time to search for long-lost echoes.",
            "An epic tale of sky-high heists and daring escapades.",
            "A deadly legacy that poisons everything in its path.",
            "A dark and mysterious world filled with shadows and secrets.",
            "A perilous quest to recover a lost treasure.",
            "A chance for redemption in the face of impossible odds.",
            "A clash of elemental forces in a frozen wasteland.",
            "A haunting tale of whispers from beyond the veil.",
            "A mystical journey through the winding forest.",
            "A cursed island filled with danger and mystery.",
            "A daring heist planned and executed by the phantom thief.",
            "An epic tale of fate and fortune on the high seas.",
            "The alchemist's greatest secret weight in the balance.",
            "An unknown kingdom with a storied past.",
            "A shadowy figure looming over all things.",
            "A lost oasis hidden away from the modern world.",
            "A treacherous journey through a dangerous crystal cavern.",
            "A broken blade that can mean everything.",
            "The secret of the sleeping sphinx remains only a mystery.",
            "An old English manor house haunted by ghosts.",
            "The dread curse of the werewolf looms over all.",
            "A garden filled with enchantment and mystery.",
            "The sands of time run ever slower.",
            "The one destined to rise above all else.",
            "The dragon queen leads her armies to battle against all foes.",
            "The wandering bard moves from town to town.",
            "The dark knight stands against all who oppose him.",
            "The last hope for the future of humanity.",
            "A daring escape from the inescapable Alcatraz.",
            "Hollow Hills is haunted by a terrifying evil.",
            "The legend of the Golden Fleece endures to this day.",
            "The magical age is a time unlike any other.",
            "A city long sunk beneath the sea fights on.",
            "The prophecy of the red moon is finally at hand.",
            "A group of hunters searches for the lost Ark.",
            "The mighty kingdom protected by a single keeper.",
            "The secrets of the alchemists remain only with their inner circle.",
            "The pirate queen sets sail on a dangerous voyage.",
            "The curse of the black pearl haunts all who seek it.",
            "An ancient sunken temple holds secrets too great to bear.",
            "The tomb of the Sphinx Queen remains hidden from all.",
            "The silence of the lambs is deafening.",
            "The abandoned asylum remains shrouded in mystery.",
            "The scandalous tale of the Duchess is too shocking for words.",
            "The missing heirloom sets off a chain of events with dire consequences.",
            "The mystery of the lost locket still remains unsolved.",
            "The shadowy depths of the underworld are not for the faint of heart.",
            "The rise of the dark lord threatens to undo all of civilization.",
            "The forbidden library's keeper is both its protector and prisoner.",
            "The cursed diamond brings nothing but misfortune to all who possess it.",
        };

        var fakeReviews = new[]
        {
            "An engaging journey through celestial mysteries with the right mix of science and human drama. A few plot holes mar an otherwise exciting narrative.",
            "A post-apocalyptic tale that challenges the norms. Vivid world-building and multidimensional characters make it a compelling read.",
            "A thoughtful exploration of human fortitude in the face of adversity. The intertwining personal stories might tug at your heartstrings, but the prose can be overly verbose.",
            "A captivating blend of romance and time-travel. Despite a few predictable turns, the love story is beautifully portrayed.",
            "Russo's powerful storytelling and empathetic characterization shine in this tale of family and forgiveness in a Sicilian village.",
            "A lucid explanation of complex quantum physics phenomena for laymen. Occasionally, the author's enthusiasm for detail can be overwhelming",
            "An ambitious, multilayered epic fantasy with exquisite world-building. Its pacing might be slow for some readers.",
            "A suspenseful thriller that effectively captures the heat and horror of a small town's darkest secrets.",
            "Greene beautifully interweaves quantum mechanics and philosophy. The science is heavy, so be prepared for a challenging but rewarding read.",
            "A moving story about the complexities of mother-daughter relationships. Shafak's beautiful prose is a pleasure to read, even when the narrative meanders.",
            "A collection of short stories that evokes a surreal, dreamlike atmosphere. Some stories, however, feel underdeveloped.",
            "A thrilling blend of ancient myths and modern mysteries. The pacing is inconsistent but the overall intrigue keeps you engaged.",
            "A thrilling exploration of artificial intelligence. While exciting, it falls into familiar Dan Brown formula.",
            "An impactful novel about the struggle for freedom and identity in post-colonial Nigeria. Adichie's writing is rich and evocative.",
            "A final gift from Hawking that wonderfully breaks down complex concepts of time and space. Occasional scientific jargon may be daunting for some.",
            "A deeply thoughtful exploration of silence and solitude. The philosophical discussions can sometimes overshadow the plot.",
            "A well-crafted tale of myths and modernity. The story meanders at times, but Gaiman's writing always shines.",
            "Another thrilling installment in the Chief Inspector Gamache series. Penny's understanding of human nature brings depth to this murder mystery.",
            "An intricately woven story of memory and identity. The slow pacing may not be for everyone, but the themes are deeply thought-provoking.",
        };

        var generatedBooks = new Faker<Book>()
                .RuleFor(b => b.Name, f => fakeBookNames[Random.Shared.Next(0, fakeBookNames.Length - 1)])
                .RuleFor(b => b.Isbn, f => f.Random.Replace("###-#-####-####-#"))
                .RuleFor(b => b.AvailableAmount, f => f.Random.Number(0, 5))
                .RuleFor(b => b.Language, f => f.PickRandom(new[] { "Czech", "Slovak", "English", "German", "French", "Spanish" }))
                .RuleFor(b => b.Description, f => fakeBookDescriptions[Random.Shared.Next(0, fakeBookDescriptions.Length - 1)])
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
                }).Generate(50);

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
            .Generate(60);

        globalIndex = 10;

        var generatedBookAuthors = new Faker<BookAuthor>()
            .RuleFor(i => i.AuthorId, i => i.PickRandom(generatedAuthors.Select(i => i.Id)))
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(70);

        foreach (var book in generatedBooks)
        {
            var bookWithoutAuthor = generatedBookAuthors.FirstOrDefault(i => book.Id == i.BookId);
            if (bookWithoutAuthor is null)
            {
                var randomNumber = f.Random.Number(15, generatedAuthors.Count - 5);
                generatedBookAuthors.Add(new BookAuthor
                {
                    BookId = book.Id,
                    AuthorId = generatedAuthors.First(i => i.Id == randomNumber).Id,
                    Id = globalIndex,
                });
                globalIndex++;
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
            .Generate(20).Concat(new Faker<User>()
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
            }).Generate(3)).ToList();

        globalIndex = 10;

        var generatedReviews = new Faker<Review>()
            .RuleFor(i => i.Rating, i => i.Random.Number(1, 5))
            .RuleFor(i => i.Text, i => f.PickRandom(fakeReviews))
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(40)
            .Concat(new Faker<Review>()
            .RuleFor(i => i.Rating, i => i.Random.Number(1, 5))
            .RuleFor(i => i.Text, i => f.PickRandom(fakeReviews))
            .RuleFor(i => i.BookId, i => i.PickRandom(generatedBooks.Select(i => i.Id)))
            .RuleFor(i => i.UserId, i => i.PickRandom(generatedUsers.Select(i => i.Id)))
            .FinishWith((i, j) =>
            {
                j.Id = globalIndex;
                globalIndex++;
            })
            .Generate(60));

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
            .Generate(100)
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
            .Generate(50))
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
            .Generate(80));

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
            .Generate(300);

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