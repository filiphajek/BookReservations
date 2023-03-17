using BookReservations.Infrastructure.BL.Common;

namespace BookReservations.Api.BL.Commands;

public record CreateUserResponse(bool Success, int? UserId = null, bool EmailExists = false, bool UserNameExists = false)
    : ErrorPropertyResponse(Success, GetErrorDictionary(EmailExists, UserNameExists))
{
    private static Dictionary<string, string[]> GetErrorDictionary(bool emailExists, bool userNameExists)
    {
        var dic = new Dictionary<string, string[]>();
        if (emailExists)
        {
            dic.Add("Email", new[] { "This email already exists" });
        }
        if (userNameExists)
        {
            dic.Add("UserName", new[] { "This username already exists" });
        }

        return dic;
    }
}
