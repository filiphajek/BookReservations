using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace BookReservations.Infrastructure.BL.Services;

public class PaginatedUrlBuilder : IPaginatedUrlBuilder
{
    private readonly HttpContext httpContext;
    private static readonly Regex pageRegex =
        new(@"(?<=page=)\d+", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(200));

    public PaginatedUrlBuilder(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext;
    }

    public string GetPaginatedUrl(int page)
    {
        var query = pageRegex.Replace(httpContext.Request.QueryString.ToString(), page.ToString());
        return $"{httpContext.Request.Path}{query}";
    }
}
