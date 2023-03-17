using BookReservations.Api.BL.Models;
using BookReservations.Api.BL.Queries;
using BookReservations.Api.Caching;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookReservations.Api.Controllers;

public class UserMiniController : MiniController
{
    protected override void AddEnpoints(RouteGroupBuilder endpoints)
    {
        endpoints.MapGet("{userId}", async ([FromRoute] int userId, IMediator mediator, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            if (httpContext.User.IsInRole(BookReservationsRoles.User))
            {
                var id = httpContext.User.GetUserId();
                return Results.Ok((await mediator.Send(new SimpleQuery<UserDetailModel, User>(i => i.Id == id!.Value), cancellationToken)).Single());
            }

            if (!httpContext.User.IsInRole(BookReservationsRoles.Librarian) && !httpContext.User.IsInRole(BookReservationsRoles.Admin))
            {
                return Results.Unauthorized();
            }
            var result = (await mediator.Send(new SimpleQuery<UserDetailModel, User>(i => i.Id == userId), cancellationToken)).Single();
            return Results.Ok(result);
        }).RequireAuthorization();

        endpoints.MapGet("cached", async (IMediator mediator, HttpContext httpContext, CancellationToken cancellationToken)
            => await mediator.Send(new SimpleQuery<UserInfoModel, User>(i => i.Role == BookReservationsRoles.User), cancellationToken))
                .RequireAuthorization(BookReservationsPolicies.ViewAllUsers)
                .CacheOutput(nameof(OutputCachePolicy));

        endpoints.MapGet("", async ([AsParameters] GetUsersContract contract, CancellationToken cancellationToken)
            => await contract.Mediator.Send(new UserPaginatedQuery(
                contract.Page, contract.PageSize, contract.OrderBy, contract.IsAscending, contract.SearchText), cancellationToken))
                    .RequireAuthorization(BookReservationsPolicies.ViewAllUsers);
    }
}
