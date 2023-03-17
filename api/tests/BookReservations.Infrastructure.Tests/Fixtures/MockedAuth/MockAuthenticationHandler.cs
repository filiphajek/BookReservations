using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace BookReservations.Infrastructure.Tests.Fixtures.MockedAuth;

public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly MockedClaims claims;

    public MockAuthenticationHandler(
        MockedClaims claims,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    )
        : base(options, logger, encoder, clock)
    {
        this.claims = claims;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var ticket = new AuthenticationTicket(claimsPrincipal, IdentityConstants.ApplicationScheme);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
