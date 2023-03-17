using BookReservations.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;

namespace BookReservations.Infrastructure.BL.Services;

public class UserIdProvider : IUserIdProvider
{
    private readonly IHttpContextAccessor accessor;

    public UserIdProvider(IHttpContextAccessor accessor)
    {
        this.accessor = accessor;
    }

    public int GetUserId()
    {
        var id = accessor.HttpContext.User.GetUserId();
        if (id is null)
        {
            throw new ArgumentNullException(nameof(id));
        }
        return id.Value;
    }
}