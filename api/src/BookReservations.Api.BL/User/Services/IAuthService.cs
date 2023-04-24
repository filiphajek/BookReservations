using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure.BL;

namespace BookReservations.Api.BL.Services;

public interface IAuthService : IService
{
    Task SignUserIn(UserModel user, string scheme = "default", CancellationToken cancellationToken = default);
    Task SignUserOut(string scheme = "default", CancellationToken cancellationToken = default);
    string GenerateJwt(UserModel user);
}
