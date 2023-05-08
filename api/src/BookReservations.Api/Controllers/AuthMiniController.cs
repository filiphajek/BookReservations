using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Facades;
using BookReservations.Api.BL.Models;
using BookReservations.Api.BL.Services;
using BookReservations.Api.DAL;
using BookReservations.Api.DAL.Entities;
using BookReservations.Api.Filters;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Common;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.BL.Services;
using BookReservations.Infrastructure.Extensions;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookReservations.Api.Controllers;

public class AuthMiniController : MiniController
{
    protected override void AddEnpoints(RouteGroupBuilder endpoints)
    {
        endpoints.MapPost("/ms-signon", async (BookReservationsDbContext dbContext, IMapper mapper, IHashService hashService, IAuthService authService, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var msUserId = httpContext.User.GetMsUserId();
            if (string.IsNullOrEmpty(msUserId))
            {
                return Results.Unauthorized();
            }

            var msUser = await dbContext.Users.FirstOrDefaultAsync(i => i.MsId == msUserId, cancellationToken: cancellationToken);
            if (msUser is not null)
            {
                var jwt = authService.GenerateJwt(mapper.Map<UserModel>(msUser));
                return Results.Ok(new UserJwtLoginResponse(true, jwt));
            }

            var name = httpContext.User.Claims.FirstOrDefault(c => c.Type == "name")!.Value.Split(' ');
            var email = httpContext.User.Claims.FirstOrDefault(c => c.Type == "preferred_username")!.Value;

            var newMsUser = new User
            {
                MsId = msUserId,
                FirstName = name.First(),
                LastName = name.Last(),
                Email = email,
                Image = "https://bookreservationsstorage.blob.core.windows.net/bookreservations/default.jpg",
                Password = hashService.Hash(DateTime.Now.ToString() + DateTime.Now.Ticks.ToString()),
                Role = BookReservationsRoles.User,
                UserName = name.First(),
            };
            await dbContext.AddAsync(newMsUser, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var njwt = authService.GenerateJwt(mapper.Map<UserModel>(newMsUser));
            return Results.Ok(new UserJwtLoginResponse(true, njwt));
        })
        .WithName("MsSignOn")
        .Produces<UserJwtLoginResponse>()
        .RequireAuthorization(BookReservationsPolicies.MsOidc);

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
            .Produces<CreateUserResponse>(400)
            .Produces<ValidationErrorResponse>(422)
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

        endpoints.MapGet("userinfo", async (IMediator mediator, IMapper mapper, HttpContext httpContext) =>
        {
            var idClaim = httpContext.User.GetUserId();
            if (idClaim is null)
            {
                return Results.BadRequest();
            }

            var user = (await mediator.Send(
                new SimpleQuery<UserModel, User>(i => i.Id == idClaim))).Single();

            var result = mapper.Map<UserInfoModel>(user);
            result.IsMsOidc = !string.IsNullOrEmpty(user.MsId);
            return Results.Ok(result);
        })
        .RequireAuthorization()
        .Produces<UserInfoModel>()
        .WithName("GetUserInfo");
    }
}
