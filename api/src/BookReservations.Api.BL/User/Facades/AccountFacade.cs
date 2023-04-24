using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Models;
using BookReservations.Api.BL.Services;
using BookReservations.Api.DAL.Entities;
using BookReservations.Infrastructure;
using BookReservations.Infrastructure.BL.Queries;
using BookReservations.Infrastructure.BL.Services;
using MapsterMapper;
using MediatR;

namespace BookReservations.Api.BL.Facades;

public class AccountFacade : IAccountFacade
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;
    private readonly IAuthService authService;
    private readonly IHashService hashService;

    public AccountFacade(IMapper mapper, IMediator mediator, IAuthService authService, IHashService hashService)
    {
        this.mapper = mapper;
        this.mediator = mediator;
        this.authService = authService;
        this.hashService = hashService;
    }

    public async Task<UserJwtLoginResponse> JwtLoginUserAsync(UserLoginModel model, CancellationToken cancellationToken = default)
    {
        var user = await TryLoginUserAsync(model, cancellationToken);
        if (user is null)
        {
            return new(false, "");
        }
        return new(true, authService.GenerateJwt(user));
    }

    public async Task<bool> LoginUserAsync(UserLoginModel model, CancellationToken cancellationToken = default)
    {
        var user = await TryLoginUserAsync(model, cancellationToken);
        if (user is null)
        {
            return false;
        }
        await authService.SignUserIn(user, cancellationToken: cancellationToken);
        return true;
    }

    public async Task LogoutUserAsync(CancellationToken cancellationToken = default)
    {
        await authService.SignUserOut(cancellationToken: cancellationToken);
    }

    public async Task<CreateUserResponse> RegisterAsync(UserModel model, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new CreateUserCommand(model), cancellationToken);
    }

    public async Task<CreateUserResponse> RegisterAsync(UserRegistrationModel model, CancellationToken cancellationToken = default)
    {
        var user = mapper.Map<UserModel>(model);
        user.Role = BookReservationsRoles.User;
        return await RegisterAsync(user, cancellationToken);
    }

    public async Task<UserModel?> TryLoginUserAsync(UserLoginModel model, CancellationToken cancellationToken = default)
    {
        var user = (await mediator.Send(new SimpleQuery<UserModel, User>(
            i => i.UserName == model.Login || i.Email == model.Login), cancellationToken)).FirstOrDefault();

        if (user is null)
        {
            return null;
        }

        if (!hashService.Verify(model.Password, user.Password))
        {
            return null;
        }

        return user;
    }

}