namespace BookReservations.Infrastructure.BL.Queries;

public record PaginatedQueryResult<TModel>(
    int Page,
    int PageSize,
    int ItemsCount,
    int TotalCount,
    ICollection<TModel> Data)
{
    public PaginatedQueryResult() : this(0, 0, 0, 0, Array.Empty<TModel>()) { }

    public string? NextPage { get; init; } = string.Empty;
    public string? PreviousPage { get; init; } = string.Empty;
}
