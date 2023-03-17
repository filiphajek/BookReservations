namespace BookReservations.Infrastructure.BL.Common;

public record ErrorResponse(bool Success, string[] Errors);
