using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Queries;
using MediatR;

namespace BookReservations.Api.Controllers;

public class ContactMiniController : MiniController
{
    protected override void AddEnpoints(RouteGroupBuilder endpoints)
    {
        endpoints.AllowAnonymous();

        endpoints.MapGet("librarians", (IMediator mediator)
            => mediator.Send(new SimpleQuery<UserModel, User>(i => i.Role == BookReservationsRoles.Librarian)));

        endpoints.MapGet("admins", (IMediator mediator)
            => mediator.Send(new SimpleQuery<UserModel, User>(i => i.Role == BookReservationsRoles.Admin)));
    }
}
