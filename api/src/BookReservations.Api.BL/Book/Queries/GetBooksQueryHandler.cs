using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Handlers;
using BookReservations.Infrastructure.BL.Services;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.BL.Queries;

public class GetBooksQueryHandler : PaginatedQueryHandler<GetBooksQuery, BookModel, Book, int>
{
    public GetBooksQueryHandler(IMapper mapper, IQuery<Book> query, IPaginatedUrlBuilder urlBuilder)
        : base(mapper, query, urlBuilder)
    {
    }

    public override IPageQuery<Book> BuildQuery(GetBooksQuery request)
    {
        var appliedQuery = ApplyMainFilters(query, request);
        if (request.OnlyAvailable)
        {
            return appliedQuery.AndWhere(i => i.AvailableAmount > 0);
        }
        return appliedQuery;
    }

    private static IAfterWhereQuery<Book> ApplyMainFilters(IQuery<Book> query, GetBooksQuery request)
    {
        var isSearchingByName = request.SearchText.Length > 4;
        var isFilteringByAuthors = request.AuthorIds.Any();
        var text = request.SearchText.ToLower();

        if (!isSearchingByName && !isFilteringByAuthors)
        {
            return query.WhereIfNotNull(null);
        }

        if (isSearchingByName && !isFilteringByAuthors)
        {
            return query.Where(i => i.Name.ToLower().Contains(text)).OrWhere(i => i.Isbn.ToLower().Contains(text));
        }

        var join = query
            .Join(i => i.Include(j => j.BookAuthors))
            .Where(i => i.BookAuthors.Any(j => request.AuthorIds.Contains(j.AuthorId)));

        if (!isSearchingByName && isFilteringByAuthors)
        {
            return join;
        }
        return join.AndWhere(i => i.Name.ToLower().Contains(text)).OrWhere(i => i.Isbn.ToLower().Contains(text));
    }
}