using BookReservations.Api.BL.Models;
using BookReservations.Infrastructure.BL.Queries;

namespace BookReservations.Api.BL.Queries;

public record GetUserBookInfoQuery(int UserId, int BookId) : QueryRequest<BookUserInfoModel>;