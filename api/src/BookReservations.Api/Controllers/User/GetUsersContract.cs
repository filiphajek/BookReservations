using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookReservations.Api.Controllers;

public record GetUsersContract(
    [FromServices] IMediator Mediator,
    [FromQuery] int Page = 1,
    [FromQuery] int PageSize = 10,
    [FromQuery] string OrderBy = "id",
    [FromQuery] bool IsAscending = true,
    [FromQuery] string SearchText = "");
