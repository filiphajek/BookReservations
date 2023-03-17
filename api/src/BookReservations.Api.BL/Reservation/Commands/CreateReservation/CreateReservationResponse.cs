using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure.BL.Common;

namespace BookReservations.Api.BL.Commands;

public record CreateReservationResponse(bool Success, int? ReservationId = null, bool BookNotAvailable = false, bool BookNotExists = false)
    : ErrorPropertyResponse(Success, GetErrorDictionary(BookNotAvailable, BookNotExists))
{
    private static Dictionary<string, string[]> GetErrorDictionary(bool bookNotAvailable, bool bookNotExists)
    {
        var dic = new Dictionary<string, string[]>();
        if (bookNotAvailable)
        {
            dic.Add(nameof(MakeReservationModel.BookId), new[] { "Book is not available" });
        }
        if (bookNotExists)
        {
            dic.Add(nameof(MakeReservationModel.BookId), new[] { "Book with this id is not in a our database" });
        }

        return dic;
    }
}