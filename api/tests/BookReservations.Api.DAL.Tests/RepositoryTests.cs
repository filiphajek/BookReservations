using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookReservations.Api.DAL.Tests;

public class RepositoryTests : BookReservationsDbContextFixture
{
    public RepositoryTests(Infrastructure.Tests.Factories.IDbContextFactory<BookReservationsDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    [Fact]
    public async Task TestGetSeededBooksSuccessful()
    {
        var repository = UnitOfWork.GetRepository<Book>();
        var books = await repository.GetAsync();
        Assert.Equal(2, books.Count);
    }

    [Fact]
    public async Task TestExistsAuthorFromSeededSuccessful()
    {
        var repository = UnitOfWork.GetRepository<Author>();
        var exists = await repository.ExistsByIdAsync(1);
        Assert.True(exists);
    }

    [Fact]
    public async Task TestInsertBookSuccessful()
    {
        var repository = UnitOfWork.GetRepository<Book>();
        var toBeInsertedBook = new Book()
        {
            Isbn = "978-11111111111",
            Name = "Test book",
            Description = "This is a test book.",
            AvailableAmount = 10
        };
        var insertedBook = await repository.InsertAsync(toBeInsertedBook);
        await UnitOfWork.CommitAsync();

        Assert.True(await repository.ExistsByIdAsync(insertedBook.Id));
    }

    [Fact]
    public async Task TestInsertBookExistingIdThrowsException()
    {
        var repository = UnitOfWork.GetRepository<Book>();
        var newBook = new Book()
        {
            Id = 1,
            Isbn = "978-11111111111",
            Name = "Test book",
            Description = "This is a test book.",
            AvailableAmount = 10
        };
        await Assert.ThrowsAsync<DbUpdateException>(() =>
        {
            repository.InsertAsync(newBook);
            return UnitOfWork.CommitAsync();
        });
    }

    [Fact]
    public async Task TestUpdateAuthorSuccessful()
    {
        var repository = UnitOfWork.GetRepository<Author>();
        var author = await repository.SingleByIdAsync(1);
        Assert.Equal("Jan", author!.FirstName);
        author.FirstName = "Honza";
        await repository.UpdateAsync(author);
        await UnitOfWork.CommitAsync();

        var newAuthor = await repository.SingleByIdAsync(1);
        Assert.Equal("Honza", newAuthor!.FirstName);
    }

    [Fact]
    public async Task TestSingleByIdBookSuccessful()
    {
        var repository = UnitOfWork.GetRepository<Book>();
        var book = await repository.SingleByIdAsync(1);
        Assert.Equal("978-0001112222", book!.Isbn);
    }

    [Fact]
    public async Task TestSingleNotExistingReturnsNull()
    {
        var repository = UnitOfWork.GetRepository<Book>();
        var nonExistentBook = await repository.SingleByIdAsync(10000);
        Assert.Null(nonExistentBook);
    }

    [Fact]
    public async Task TestRemoveFromAuthorsSuccessful()
    {
        var repository = UnitOfWork.GetRepository<Author>();
        var author = await repository.SingleByIdAsync(1);
        await repository.RemoveAsync(author!);
        await UnitOfWork.CommitAsync();
        Assert.False(await repository.ExistsByIdAsync(1));
    }

    [Fact]
    public async Task TestBookToBookAuthorCascadeDelete()
    {
        var bookRepository = UnitOfWork.GetRepository<Book>();
        var book = await bookRepository.SingleByIdAsync(1);
        await bookRepository.RemoveAsync(book!);
        await UnitOfWork.CommitAsync();

        var bookAuthorRepository = UnitOfWork.GetRepository<BookAuthor>();
        Assert.False(await bookAuthorRepository.ExistsByIdAsync(1));
        Assert.False(await bookAuthorRepository.ExistsByIdAsync(3));
        Assert.False(await bookAuthorRepository.ExistsByIdAsync(4));
    }

    [Fact]
    public async Task TestAuthorToBookAuthorCascadeDelete()
    {
        var authorRepository = UnitOfWork.GetRepository<Author>();
        var author = await authorRepository.SingleByIdAsync(1);
        await authorRepository.RemoveAsync(author!);
        await UnitOfWork.CommitAsync();

        var bookAuthorRepository = UnitOfWork.GetRepository<BookAuthor>();
        Assert.False(await bookAuthorRepository.ExistsByIdAsync(1));
        Assert.False(await bookAuthorRepository.ExistsByIdAsync(2));
    }

    [Fact]
    public async Task TestUserCascadeDelete()
    {
        var userRepository = UnitOfWork.GetRepository<User>();
        var user = await userRepository.SingleByIdAsync(4);
        await userRepository.RemoveAsync(user!);
        await UnitOfWork.CommitAsync();

        var userBookRelationsRepository = UnitOfWork.GetRepository<UserBookRelations>();
        Assert.False(await userBookRelationsRepository.ExistsByIdAsync(1));
    }

    [Fact]
    public async Task TestReviewSetNullDelete()
    {
        var userRepository = UnitOfWork.GetRepository<User>();
        var user = await userRepository.SingleByIdAsync(4);
        await userRepository.RemoveAsync(user!);
        await UnitOfWork.CommitAsync();

        var reviewRepository = UnitOfWork.GetRepository<Review>();
        Assert.True(await reviewRepository.ExistsByIdAsync(1));

        var review = await reviewRepository.SingleByIdAsync(1);
        Assert.Null(review!.UserId);
    }
}
