using BookReservations.Infrastructure.BL.Common;

namespace BookReservations.Api.BL.Commands;

public record UpdateUserResponse : ErrorPropertyResponse
{
    public UpdateUserResponse(bool Success, Dictionary<string, string[]> Errors) : base(Success, Errors)
    {
    }

    public UpdateUserResponse(string property, string error) : base(property, error)
    {
    }
}
