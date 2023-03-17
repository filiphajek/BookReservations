using System.Linq.Expressions;

namespace BookReservations.Infrastructure.BL.Queries;

public record SimpleQuery<TModel, TEntity>(Expression<Func<TEntity, bool>>? Predicate = null) : QueryRequest<ICollection<TModel>>;