using System.Linq.Expressions;

namespace BookReservations.Infrastructure.BL.Commands;

public record DeleteCommand<TEntity>(Expression<Func<TEntity, bool>> Predicate) : CommandRequest;
