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
}
