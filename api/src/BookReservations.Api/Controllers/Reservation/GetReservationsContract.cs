using BookReservations.Api.DAL.Enums;

namespace BookReservations.Api.Controllers;

public record GetReservationsContract(
    ICollection<int> UserIds,
    ICollection<int> BookIds,
    ICollection<ReservationStatus> Statuses,
    int Page = 1,
    int PageSize = 10,
    string OrderBy = "id",
    bool IsAscending = true);