using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Facades;
using BookReservations.Api.BL.Models;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.Filters;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.Extensions;
using MediatR;

namespace BookReservations.Api.Controllers;

public class AuthMiniController : MiniController
{
    protected override void AddEnpoints(RouteGroupBuilder endpoints)
    {
        endpoints.MapPost("/register", async (UserRegistrationModel model, IAccountFacade facade, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            if (httpContext.User.Identity is not null && httpContext.User.Identity.IsAuthenticated)
            {
                return Results.BadRequest();
            }

            var result = await facade.RegisterAsync(model, cancellationToken);
            if (result.Success)
            {
                return Results.Ok(result);
            }

            return Results.BadRequest(result);
        })
        .Produces<CreateUserResponse>()
        .ProducesProblem(404)
        .WithName("Register")
        .AddEndpointFilter<ValidationFilter<UserRegistrationModel>>();

        endpoints.MapPost("login", async (UserLoginModel model, IAccountFacade facade, CancellationToken cancellationToken) =>
        {
            if (!await facade.LoginUserAsync(model, cancellationToken))
            {
                return Results.BadRequest();
            }

            return Results.Ok();
        })
        .Produces(200)
        .ProducesProblem(404)
        .WithName("Login")
        .AddEndpointFilter<ValidationFilter<UserLoginModel>>();

        endpoints.MapPost("loginjwt", async (UserLoginModel model, IAccountFacade facade, CancellationToken cancellationToken) =>
        {
            var login = await facade.JwtLoginUserAsync(model, cancellationToken);
            if (!login.Success)
            {
                return Results.BadRequest(login);
            }
            return Results.Ok(login);
        })
        .Produces<UserJwtLoginResponse>(200)
        .ProducesProblem(404)
        .WithName("LoginJwt")
        .AddEndpointFilter<ValidationFilter<UserLoginModel>>();

        endpoints.MapPost("logout", async (IAccountFacade facade, CancellationToken cancellationToken) =>
        {
            await facade.LogoutUserAsync(cancellationToken);
            return Results.Ok();
        })
        .RequireAuthorization()
        .WithName("Logout");

        endpoints.MapGet("userinfo", async (IMediator mediator, HttpContext httpContext) =>
        {
            var idClaim = httpContext.User.GetUserId();
            if (idClaim is null)
            {
                return Results.BadRequest();
            }

            var result = (await mediator.Send(
                new SimpleQuery<UserInfoModel, User>(i => i.Id == idClaim))).Single();
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .Produces<UserInfoModel>()
        .WithName("GetUserInfo");
    }
}
