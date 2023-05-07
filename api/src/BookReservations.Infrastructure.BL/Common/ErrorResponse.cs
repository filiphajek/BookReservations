namespace BookReservations.Infrastructure.BL.Common;

public record ErrorResponse(bool Success, string[] Errors);
public record ValidationErrorResponse(string Title, string Type, int Status, Dictionary<string, string[]> Errors);

