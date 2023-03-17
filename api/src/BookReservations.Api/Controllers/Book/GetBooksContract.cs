namespace BookReservations.Api.Controllers;

public record GetBooksContract(
    ICollection<int>? AuthorIds = null,
    bool OnlyAvailable = false,
    int Page = 1,
    int PageSize = 10,
    string OrderBy = "id",
    bool IsAscending = true,
    string SearchText = "");
