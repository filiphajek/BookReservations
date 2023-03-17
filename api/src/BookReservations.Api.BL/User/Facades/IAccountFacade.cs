using BookReservations.Api.BL.Commands;
using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure.BL;

namespace BookReservations.Api.BL.Facades;

public interface IAccountFacade : IFacade
{
    Task<CreateUserResponse> RegisterAsync(UserRegistrationModel model, CancellationToken cancellationToken = default);
    Task<CreateUserResponse> RegisterAsync(UserModel model, CancellationToken cancellationToken = default);
    Task<bool> LoginUserAsync(UserLoginModel model, CancellationToken cancellationToken = default);
    Task LogoutUserAsync(CancellationToken cancellationToken = default);
}
