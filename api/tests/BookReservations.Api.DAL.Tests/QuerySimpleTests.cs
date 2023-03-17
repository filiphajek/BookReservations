using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Tests.Fixtures;
using BookReservations.Infrastructure.DAL;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookReservations.Api.DAL.Tests;

[Collection(nameof(QueryTests))]
public class QuerySimpleTests
{
    private readonly IUnitOfWork uow;

    public QuerySimpleTests(BookReservationsQueryFixture fixture)
    {
        uow = fixture.UnitOfWork;
    }

    [Fact]
    public async Task TestPaging()
    {
        var result = await uow.Query<BookAuthor>()
            .Page(0, pageSize: 5)
            .ExecuteAsync();

        Assert.Equal(5, result.ItemsCount);
        Assert.Null(result.TotalCount);
        Assert.Equal(0, result.Page);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task TestPagingWithTotalCount()
    {
        var result = await uow.Query<BookAuthor>()
            .Page(0, pageSize: 5, includeTotalCount: true)
            .ExecuteAsync();

        Assert.Equal(5, result.ItemsCount);
        Assert.Equal(18, result.TotalCount);
        Assert.Equal(0, result.Page);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task TestPagingLastPage()
    {
        var result = await uow.Query<BookAuthor>()
            .Page(4, pageSize: 5)
            .ExecuteAsync();

        Assert.Equal(3, result.ItemsCount);
        Assert.Null(result.TotalCount);
        Assert.Equal(4, result.Page);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task TestOrderByAscending()
    {
        var result = await uow.Query<Author>()
            .OrderBy(author => author.FirstName, ascendingOrder: true)
            .ExecuteAsync();

        Assert.Equal(10, result.ItemsCount);
        var authorNames = result.Data.Select(author => author.FirstName).ToList();
        var authorNamesCopy = new List<string>(authorNames);
        authorNames.OrderBy(aname => aname);

        Assert.Equal(authorNamesCopy, authorNames);
    }

    [Fact]
    public async Task TestOrderByDescending()
    {
        var result = await uow.Query<Author>()
            .OrderBy(author => author.FirstName, ascendingOrder: false)
            .ExecuteAsync();

        Assert.Equal(10, result.ItemsCount);
        var authorNames = result.Data.Select(author => author.FirstName).ToList();
        var authorNamesCopy = new List<string>(authorNames);
        authorNames.OrderByDescending(aname => aname);

        Assert.Equal(authorNamesCopy, authorNames);
    }

    [Fact]
    public async Task TestWhere()
    {
        var result = await uow.Query<Author>()
            .Where(author => author.Nationality.Equals("German"))
            .ExecuteAsync();

        Assert.Equal(3, result.ItemsCount);
        Assert.Contains("Ursula", result.Data.Select(author => author.FirstName));
        Assert.Contains("Thomas", result.Data.Select(author => author.FirstName));
        Assert.Contains("Karl", result.Data.Select(author => author.FirstName));
    }

    [Fact]
    public async Task TestAndWhere()
    {
        var result = await uow.Query<Book>()
            .Where(book => book.Description.Equals("noDesc"))
            .AndWhere(book => book.AvailableAmount > 55)
            .ExecuteAsync();

        Assert.Equal(1, result.ItemsCount);
        Assert.Contains("queryTestBookName2", result.Data.Select(book => book.Name));
    }

    [Fact]
    public async Task TestOrWhere()
    {
        var result = await uow.Query<Book>()
            .Where(book => book.Description.Equals("longDescription"))
            .OrWhere(book => book.Isbn.Equals("978-0000033333"))
            .ExecuteAsync();

        Assert.Equal(3, result.ItemsCount);
        Assert.Contains("queryTestBookName3", result.Data.Select(book => book.Name));
        Assert.Contains("queryTestBookName5", result.Data.Select(book => book.Name));
        Assert.Contains("queryTestBookName6", result.Data.Select(book => book.Name));

    }

    [Fact]
    public async Task TestJoin()
    {
        var result = await uow.Query<Author>()
            .Join(i => i.Include(i => i.Books))
            .Where(author => author.Books.Count == 0)
            .ExecuteAsync();

        Assert.Equal(1, result.ItemsCount);
        Assert.Contains("Hoffman", result.Data.Select(author => author.LastName));
    }
}