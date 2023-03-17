using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Tests.Fixtures;
using BookReservations.Infrastructure.DAL;
using Xunit;

namespace BookReservations.Api.DAL.Tests;

[Collection(nameof(QueryTests))]
public class QueryComplexTests
{
    private readonly IUnitOfWork uow;

    public QueryComplexTests(BookReservationsQueryFixture fixture)
    {
        uow = fixture.UnitOfWork;
    }

    [Fact]
    public async Task TestBookAuthorJoinOnBookAndAuthorWithWhereOrderByAndPageMethods()
    {
        var result = await uow.Query<BookAuthor>()
            .Join(nameof(BookAuthor.Book), nameof(BookAuthor.Author))
            .Where(i => i.Book.Id != 6)
            .OrderBy(i => i.AuthorId, false)
            .Page(0, 14)
            .ExecuteAsync();

        Assert.True(result.Data.Count <= 14);
        Assert.Equal(9, result.Data.First().AuthorId);
        Assert.Equal(2, result.Data.Last().AuthorId);
        Assert.True(result.Data.All(i => i.BookId != 6));
        Assert.Equal(8, result.Data.Select(ba => ba.Author).DistinctBy(i => i.Id).Count());
        Assert.Equal(5, result.Data.Select(ba => ba.Book).DistinctBy(i => i.Id).Count());
    }

    [Fact]
    public async Task TestBookAscOrderByPaging()
    {
        var result = await uow.Query<Book>()
            .OrderBy(book => book.Description.Length)
            .Page(1, pageSize: 2)
            .ExecuteAsync();

        Assert.True(result.Data.Count == 2);
        Assert.Contains("queryTestBookName1", result.Data.Select(a => a.Name).ToList());
        Assert.Contains("queryTestBookName2", result.Data.Select(a => a.Name).ToList());
    }

    [Fact]
    public async Task TestAndOrWhere()
    {
        var firstResult = await uow.Query<Author>()
            .Where(i => i.Id != 6)
            .AndWhere(i => i.LastName == "Soukup")
            .OrWhere(i => i.LastName == "Norman")
            .ExecuteAsync();

        Assert.Equal(4, firstResult.ItemsCount);

        var secondResult = await uow.Query<Author>()
            .Where(i => i.Id != 6)
            .AndWhere(i => i.LastName == "Soukup")
            .OrWhere(i => i.LastName == "Norman")
            .AndWhere(i => i.FirstName == "Jan" || i.FirstName == "Thomas")
            .ExecuteAsync();

        Assert.Equal(2, secondResult.ItemsCount);
        Assert.Contains("Jan", secondResult.Data.Select(i => i.FirstName));
        Assert.Contains("Norman", secondResult.Data.Select(i => i.LastName));
    }
}