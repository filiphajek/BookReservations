using System.Linq.Expressions;

namespace BookReservations.Infrastructure.BL.Queries;

public record PaginatedQuery<TModel, TEntity>(
    int Page,
    int PageSize = 10,
    string SortBy = "id",
    bool Ascending = true,
    Expression<Func<TEntity, bool>>? Predicate = null)
    : QueryRequest<PaginatedQueryResult<TModel>>;
