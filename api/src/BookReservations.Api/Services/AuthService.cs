using BookReservations.Api.BL.Models;
using BookReservations.Api.BL.Services;
using BookReservations.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookReservations.Api.Services;

public class AuthService : IAuthService
{
    private readonly static JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    private readonly HttpContext httpContext;
    private readonly AuthServiceOptions authServiceOptions;

    public AuthService(IHttpContextAccessor httpContextAccessor, IOptions<AuthServiceOptions> options)
    {
        httpContext = httpContextAccessor.HttpContext!;
        authServiceOptions = options.Value;
    }

    public string GenerateJwt(UserModel user)
    {
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authServiceOptions.SecretKey)),
            SecurityAlgorithms.HmacSha512);

        return jwtSecurityTokenHandler.WriteToken(new JwtSecurityToken(
            authServiceOptions.Issuer,
            authServiceOptions.Audience,
            new Claim[]
            {
                new Claim(BookReservationsClaimTypes.UserId, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            },
            null,
            DateTime.UtcNow.AddHours(1), credentials));

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
