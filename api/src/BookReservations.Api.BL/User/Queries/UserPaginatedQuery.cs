using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure.BL.Queries;

namespace BookReservations.Api.BL.Queries;

public record UserPaginatedQuery(
    int Page,
    int PageSize = 10,
    string SortBy = "id",
    bool Ascending = true,
    string SearchText = "")
    : PaginatedQuery<UserInfoModel, User>(Page, PageSize, SortBy, Ascending, null);
