using BookReservations.Infrastructure.DAL.Query.Interfaces;
using System.Linq.Expressions;

namespace BookReservations.Infrastructure.DAL.Query;

public abstract class Query<TEntity> : IQuery<TEntity>, IAfterWhereQuery<TEntity>, IAfterJoinQuery<TEntity>
    where TEntity : class, IBaseEntity
{
    protected (int Page, int PageSize)? PaginationContainer { get; set; }
    protected (Expression<Func<TEntity, object>>? KeySelector, bool AscendingOrder, string? PropertyName)? OrderByContainer { get; set; }
    protected List<(Expression<Func<TEntity, bool>> Expression, bool IsAnd)> PredicateContainer { get; set; } = new();
    protected (List<string> TableNames, bool Invoked) JoinContainer { get; set; } = new();
    protected bool IncludeTotalCount { get; set; }

    public IQueryExecution<TEntity> Page(int page, int pageSize = 10, bool includeTotalCount = false)
    {
        PaginationContainer = (page, pageSize);
        IncludeTotalCount = includeTotalCount;
        return this;
    }

    public IPageQuery<TEntity> OrderBy(Expression<Func<TEntity, object>> keySelector, bool ascendingOrder = true)
    {
        OrderByContainer = (keySelector, ascendingOrder, null);
        return this;
    }

    public IPageQuery<TEntity> OrderBy(string propertyName, bool ascendingOrder = true)
    {
        OrderByContainer = (null, ascendingOrder, propertyName);
        return this;
    }

    public IAfterWhereQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        PredicateContainer.Add(new(predicate, true));
        return this;
    }

    public IAfterWhereQuery<TEntity> AndWhere(Expression<Func<TEntity, bool>> predicate)
    {
        PredicateContainer.Add(new(predicate, true));
        return this;
    }

    public IAfterWhereQuery<TEntity> OrWhere(Expression<Func<TEntity, bool>> predicate)
    {
        PredicateContainer.Add(new(predicate, false));
        return this;
    }

    public IAfterJoinQuery<TEntity> Join(params string[] tableNames)
    {
        JoinContainer = new(tableNames.Distinct().ToList(), true);
        return this;
    }

    public abstract Task<QueryResult<TEntity>> ExecuteAsync(CancellationToken cancellationToken = default);
}