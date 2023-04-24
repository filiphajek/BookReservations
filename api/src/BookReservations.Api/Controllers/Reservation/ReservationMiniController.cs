using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.DAL.Enums;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Common;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BookReservations.Api.Controllers;

public class ReservationMiniController : MiniController
{
    protected override void AddEnpoints(RouteGroupBuilder endpoints)
    {
        endpoints.MapPost("", async ([FromBody] GetReservationsContract contract, IMediator mediator, HttpContext HttpContext, CancellationToken cancellationToken) =>
        {
            var isUser = HttpContext.User.IsInRole(BookReservationsRoles.User);
            var userIds = contract.UserIds.ToList();
            if (isUser)
            {
                var id = HttpContext.User.GetUserId();
                userIds = new List<int>() { id!.Value };
            }

            Expression<Func<Reservation, bool>> Predicate = ExpressionExtensions.True<Reservation>();
            if (userIds.Any())
            {
                Predicate = i => userIds.Contains(i.UserId);
            }
            if (contract.BookIds.Any())
            {
                Predicate = Predicate.And(i => contract.BookIds.Contains(i.BookId));
            }
            if (contract.Statuses.Any())
            {
                Predicate = Predicate.And(i => contract.Statuses.Contains(i.Status));
            }
            var result = await mediator.Send(new PaginatedQuery<ReservationModel, Reservation>(
                contract.Page, contract.PageSize, contract.OrderBy, contract.IsAscending, Predicate), cancellationToken);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .WithName("GetReservations")
        .Produces<PaginatedQueryResult<ReservationModel>>();

        endpoints.MapPost("{reservationId}/cancel", async ([FromRoute] int reservationId, IMediator meditor, CancellationToken cancellationToken) =>
        {
            var result = await meditor.Send(
                new UpdateReservationStatusCommand(new List<UpdateReservationModel>() { new UpdateReservationModel
                {
                    Id = reservationId,
                    Status = ReservationStatus.Cancelled
                } }), cancellationToken);

            if (result.Success)
            {
                return Results.Ok();
            }
            return Results.BadRequest(result);
        })
        .RequireAuthorization(BookReservationsPolicies.UserPolicy)
        .WithName("CancelReservation")
        .Produces<ErrorPropertyResponse>(400)
        .Produces(200);

        endpoints.MapPost("{reservationId}/extend", async ([FromRoute] int reservationId, IMediator meditor, CancellationToken cancellationToken) =>
        {
            var result = await meditor.Send(
                new UpdateReservationStatusCommand(new List<UpdateReservationModel>() { new UpdateReservationModel
                {
                    Id = reservationId,
                    Status = ReservationStatus.WantToExtend,
                } }), cancellationToken);

            if (result.Success)
            {
                return Results.Ok();
            }
            return Results.BadRequest(result);
        })
        .RequireAuthorization(BookReservationsPolicies.UserPolicy)
        .WithName("ExtendReservation")
        .Produces<ErrorPropertyResponse>(400)
        .Produces(200);

        endpoints.MapPut("", async ([FromBody] ICollection<UpdateReservationModel> reservations, IMediator meditor, CancellationToken cancellationToken) =>
        {
            var result = await meditor.Send(
                new UpdateReservationStatusCommand(reservations), cancellationToken);

            if (result.Success)
            {
                return Results.Ok();
            }
            return Results.BadRequest(result);
        })
        .RequireAuthorization(BookReservationsPolicies.CanUpdateBookReservationPolicy)
        .WithName("UpdateReservations")
        .Produces<ErrorPropertyResponse>(400)
        .Produces(200);
    }
}
