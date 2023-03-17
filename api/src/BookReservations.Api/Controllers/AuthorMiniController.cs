using BookReservations.Api.BL.Models;
using BookReservations.Api.Caching;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.Filters;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.BL.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookReservations.Api.Controllers;

public class AuthorMiniController : MiniController
{
    protected override void AddEnpoints(RouteGroupBuilder endpoints)
    {
        endpoints.AllowAnonymous();

        endpoints.MapGet("", async (IMediator mediator, CancellationToken cancellationToken)
            => await mediator.Send(new SimpleQuery<AuthorModel, Author>(), cancellationToken));

        endpoints.MapGet("cached", async (IMediator mediator, CancellationToken cancellationToken)
            => await mediator.Send(new SimpleQuery<AuthorSimpleModel, Author>(), cancellationToken))
                .CacheOutput(nameof(OutputCachePolicy));

        endpoints.MapPost("", async ([FromBody] AuthorModel author, IMediator mediator, CancellationToken cancellationToken)
            => (await mediator.Send(new SetCommand<AuthorModel>(new[] { author }), cancellationToken)).FirstOrDefault())
                .RequireAuthorization(BookReservationsPolicies.CanUpdateBookReservationPolicy)
                .AddEndpointFilter<ValidationFilter<AuthorModel>>();
    }
}
