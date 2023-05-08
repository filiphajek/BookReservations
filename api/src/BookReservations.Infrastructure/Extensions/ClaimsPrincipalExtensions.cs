using System.Security.Claims;

namespace BookReservations.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var claim = claimsPrincipal.Claims.FirstOrDefault(i => i.Type == BookReservationsClaimTypes.UserId);
        if (claim is null)
        {
            return null;
        }

        if (!int.TryParse(claim.Value, out var result))
        {
            return null;

        }
        return result;
    }

    public static string GetMsUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var claim = claimsPrincipal.Claims.FirstOrDefault(i => i.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier");
        if (claim is null || string.IsNullOrEmpty(claim.Value))
        {
            return string.Empty;
        }
        return claim.Value;
    }
}
