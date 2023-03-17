using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Models;
using BookReservations.Api.BL.Queries;
using BookReservations.Api.Caching;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookReservations.Api.Controllers;

public class BookMiniController : MiniController
{
    protected override void AddEnpoints(RouteGroupBuilder endpoints)
    {
        endpoints.MapGet("cached", async (IMediator mediator, HttpContext httpContext, CancellationToken cancellationToken)
            => await mediator.Send(new SimpleQuery<BookModel, Book>(), cancellationToken))
            .AllowAnonymous()
            .CacheOutput(nameof(OutputCachePolicy));

        endpoints.MapPost("filter", async ([FromBody] GetBooksContract contract, HttpContext httpContext, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId is not null && httpContext.User.IsInRole(BookReservationsRoles.User))
            {
                var booksWithUserInfo = await mediator.Send(new GetBooksWithUserInfoQuery(
                    userId.Value, contract.Page, contract.AuthorIds ?? new List<int>(), contract.OnlyAvailable, contract.PageSize, contract.OrderBy, contract.IsAscending, contract.SearchText),
                        cancellationToken);
                return Results.Ok(booksWithUserInfo);
            }
            var books = await mediator.Send(new GetBooksQuery(
                contract.Page, contract.AuthorIds ?? new List<int>(), contract.OnlyAvailable, contract.PageSize, contract.OrderBy, contract.IsAscending, contract.SearchText),
                    cancellationToken);
            return Results.Ok(books);
        }).AllowAnonymous();

        endpoints.MapPut("", async ([FromBody] ICollection<BookModel> models, IMediator mediator, CancellationToken cancellationToken)
            => await mediator.Send(new UpdateBooksCommand(models),
                cancellationToken)).RequireAuthorization(BookReservationsPolicies.CanUpdateBookReservationPolicy);

        endpoints.MapGet("{bookId}", async ([FromRoute] int bookId, IMediator mediator, CancellationToken cancellationToken)
            => (await mediator.Send(new SimpleQuery<BookDetailModel, Book>(i => i.Id == bookId),
                cancellationToken)).FirstOrDefault()).AllowAnonymous();

        endpoints.MapGet("{bookId}/userInfo", async ([FromRoute] int bookId, IMediator mediator, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            var result = await mediator.Send(new GetUserBookInfoQuery(userId.Value, bookId), cancellationToken);
            return Results.Ok(result);
        }).RequireAuthorization(BookReservationsPolicies.UserPolicy);

        endpoints.MapGet("{bookId}/isAvailable", async ([FromRoute] int bookId, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = (await mediator.Send(new SimpleQuery<BookModel, Book>(i => i.Id == bookId), cancellationToken)).FirstOrDefault();
                if (result is null)
                {
                    return Results.NotFound();
                }
                return result.IsAvailable ? Results.Ok() : Results.NoContent();
            }).RequireAuthorization();

        endpoints.MapPost("reservation", async ([FromBody] MakeReservationModel reservation, IMediator mediator, CancellationToken cancellationToken)
            => await mediator.Send(new CreateReservationCommand(reservation),
                cancellationToken)).RequireAuthorization(BookReservationsPolicies.UserPolicy);

        endpoints.MapDelete("{bookId}/reviews/{reviewId}", async ([FromRoute] int reviewId, IMediator mediator, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            if (httpContext.User.IsInRole(BookReservationsRoles.User))
            {
                var id = httpContext.User.GetUserId();
                if (id is null)
                {
                    return Results.Unauthorized();
                }
                await mediator.Send(new DeleteCommand<Review>(i => i.UserId == id.Value && i.Id == reviewId), cancellationToken);
                return Results.NoContent();
            }
            await mediator.Send(new DeleteCommand<Review>(i => i.Id == reviewId), cancellationToken);
            return Results.NoContent();
        }).RequireAuthorization();

        endpoints.MapPost("{bookId}/reviews", async ([FromBody] ReviewModel model, IMediator mediator, HttpContext htttpContext, CancellationToken cancellationToken) =>
        {
            var userId = htttpContext.User.GetUserId();
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            if (userId != model.UserId)
            {
                return Results.Unauthorized();
            }

            var result = await mediator.Send(new SetCommand<ReviewModel>(new[] { model }), cancellationToken);
            if (result.Any())
            {
                return Results.Ok(result);
            }
            return Results.NoContent();
        }).RequireAuthorization(BookReservationsPolicies.UserPolicy);
    }
}
