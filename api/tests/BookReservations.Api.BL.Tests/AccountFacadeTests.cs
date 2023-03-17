using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Facades;
using BookReservations.Api.BL.Models;
using BookReservations.Api.BL.Services;
using BookReservations.Infrastructure.BL.Services;
using MapsterMapper;
using MediatR;
using NSubstitute;
using Xunit;

namespace BookReservations.Api.BL.Tests;

public class AccountFacadeTests
{
    private readonly IMapper mapper = Substitute.For<IMapper>();
    private readonly IMediator mediator = Substitute.For<IMediator>();
    private readonly IAuthService authService = Substitute.For<IAuthService>();
    private readonly IHashService hashService = Substitute.For<IHashService>();

    [Fact]
    public async Task LoginUserShouldVerifyPassword()
    {
        var facade = new AccountFacade(mapper, mediator, authService, hashService);

        var userLoginModel = new UserLoginModel { Login = "filip", Password = "12345" };
        var userModel = new UserModel { Email = "filip@gmail.com", UserName = "filip", Password = "hash" };

        mediator.Send(Arg.Any<IRequest<ICollection<UserModel>>>()).Returns(new[] { userModel });
        hashService.Verify(userLoginModel.Password, userModel.Password).Returns(true);
        authService.SignUserIn(userModel).Returns(Task.CompletedTask);

        var result = await facade.LoginUserAsync(userLoginModel);
        Assert.True(result);

        await mediator.ReceivedWithAnyArgs(1).Send(Arg.Any<IRequest<ICollection<UserModel>>>());
        hashService.Received(1).Verify(userLoginModel.Password, userModel.Password);
        await authService.Received(1).SignUserIn(userModel);
    }

    [Fact]
    public async Task RegisterUserShouldHashPassword()
    {
        var facade = new AccountFacade(mapper, mediator, authService, hashService);

        var userModel = new UserModel { Email = "filip@gmail.com", UserName = "filip", Password = "12345" };
        var response = new CreateUserResponse(true);

        hashService.Hash(userModel.Password).Returns("hash");
        mediator.Send(Arg.Any<IRequest<CreateUserResponse>>()).Returns(response);

        var result = await facade.RegisterAsync(userModel);
        Assert.True(result.Success);
        Assert.Equal("12345", userModel.Password);
        await mediator.ReceivedWithAnyArgs(1).Send(Arg.Any<IRequest<CreateUserResponse>>());
    }
}
