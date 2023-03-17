using BookReservations.Api.BL.Models;
using BookReservations.Api.BL.Services;
using BookReservations.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BookReservations.Api.Services;

public class AuthService : IAuthService
{
    private readonly HttpContext httpContext;
    private readonly AuthServiceOptions authServiceOptions;

    public AuthService(IHttpContextAccessor httpContextAccessor, IOptions<AuthServiceOptions> options)
    {
        httpContext = httpContextAccessor.HttpContext!;
        authServiceOptions = options.Value;
    }

    public async Task SignUserIn(UserModel user, string scheme = "default", CancellationToken cancellationToken = default)
    {
        await httpContext.SignInAsync(scheme, new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                    new Claim(BookReservationsClaimTypes.UserId, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
            }, scheme)), new AuthenticationProperties
            {
                IsPersistent = authServiceOptions.IsPersistent,
            });
    }

    public async Task SignUserOut(string scheme = "default", CancellationToken cancellationToken = default)
    {
        await httpContext.SignOutAsync(scheme);
    }
}
