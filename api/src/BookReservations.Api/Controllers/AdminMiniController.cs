using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.Filters;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookReservations.Api.Controllers;

public class AdminMiniController : MiniController
{
    protected override void AddEnpoints(RouteGroupBuilder endpoints)
    {
        var userGroup = endpoints
            .MapGroup("user")
            .RequireAuthorization(BookReservationsPolicies.AdminPolicy);

        userGroup.MapPost("", async (UserModel model, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new CreateUserCommand(model), cancellationToken);
            if (result.Success)
            {
                return Results.Ok(result);
            }
            return Results.BadRequest(result);
        }).AddEndpointFilter<ValidationFilter<UserModel>>();

        userGroup.MapDelete("", async ([FromBody] ICollection<int> userIds, IMediator mediator, CancellationToken cancellationToken)
            => await mediator.Send(new DeleteCommand<User>(i => userIds.Contains(i.Id)), cancellationToken));
    }
}
