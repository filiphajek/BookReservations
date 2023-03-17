using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Enums;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Commands;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookReservations.Api.Controllers;

public class RelationsMiniController : MiniController
{
    protected override void AddEnpoints(RouteGroupBuilder endpoints)
    {
        endpoints.RequireAuthorization(BookReservationsPolicies.UserPolicy);

        endpoints.MapPost("wishlist", async ([FromBody] ICollection<int> bookIds, IMediator mediator, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            var result = await AddRelation(userId.Value, bookIds, UserBookRelationType.Wishlist, mediator, cancellationToken);
            return result.Any() ? Results.Ok(result) : Results.NoContent();
        });

        endpoints.MapPost("subscription", async ([FromBody] ICollection<int> bookIds, IMediator mediator, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            var result = await AddRelation(userId.Value, bookIds, UserBookRelationType.Subscription, mediator, cancellationToken);
            return result.Any() ? Results.Ok(result) : Results.NoContent();
        });

        endpoints.MapGet("", async ([FromQuery] UserBookRelationType? relationType, IMediator mediator, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            var result = await mediator.Send(new SimpleQuery<RelationInfoModel, UserBookRelations>(relationType is null ? i => userId.Value == i.UserId :
                i => i.RelationType == relationType && userId.Value == i.UserId), cancellationToken);

            return result.Any() ? Results.Ok(result) : Results.NoContent();
        });

        endpoints.MapDelete("", async ([FromBody] ICollection<int> relationIds, IMediator mediator, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var userId = httpContext.User.GetUserId();
            if (userId is null)
            {
                return Results.Unauthorized();
            }
            var ids = relationIds.Distinct().ToArray();
            var result = await mediator.Send(new DeleteCommand<UserBookRelations>(i => ids.Contains(i.Id)), cancellationToken);
            return Results.NoContent();
        });
    }

    private static async Task<ICollection<RelationModel>> AddRelation(
        int userId, ICollection<int> bookIds, UserBookRelationType relationType, IMediator mediator, CancellationToken cancellationToken)
    {
        var existingBookRelationIds = (await mediator.Send(new SimpleQuery<RelationModel, UserBookRelations>(
            i => i.UserId == userId && i.RelationType == relationType && bookIds.Contains(i.BookId)), cancellationToken)).Select(i => i.BookId).Distinct().ToArray();

        var models = bookIds.Where(i => !existingBookRelationIds.Contains(i)).Select(i => new RelationModel
        {
            BookId = i,
            RelationType = relationType,
            UserId = userId
        });
        return await mediator.Send(new SetCommand<RelationModel>(models.ToArray()), cancellationToken);
    }
}
