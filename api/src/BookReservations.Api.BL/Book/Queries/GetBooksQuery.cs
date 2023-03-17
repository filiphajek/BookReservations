using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Queries;

namespace BookReservations.Api.BL.Queries;

public record GetBooksQuery(
    int Page,
    ICollection<int> AuthorIds,
    bool OnlyAvailable = false,
    int PageSize = 10,
    string SortBy = "id",
    bool Ascending = true,
    string SearchText = "")
    : PaginatedQuery<BookModel, Book>(Page, PageSize, SortBy, Ascending, null);
