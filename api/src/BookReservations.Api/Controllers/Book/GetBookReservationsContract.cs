using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookReservations.Api.Controllers;

public record GetBookReservationsContract(
    [FromServices]
    IMediator Mediator,
    [FromRoute]
    int BookId,
    [FromQuery]
    int Page = 1,
    [FromQuery]
    int PageSize = 10,
    [FromQuery]
    string OrderBy = "id",
    [FromQuery]
    bool IsAscending = true);
