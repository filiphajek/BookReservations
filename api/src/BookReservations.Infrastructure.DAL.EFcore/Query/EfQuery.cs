using BookReservations.Infrastructure.DAL.EFcore.Extensions;
using BookReservations.Infrastructure.DAL.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookReservations.Infrastructure.DAL.EFcore.Query;

public class EfQuery<TEntity> : Query<TEntity>, IEfQuery<TEntity>
    where TEntity : class, IBaseEntity
{
    protected Func<IQueryable<TEntity>, IQueryable<TEntity>>? Includes { get; set; }
    protected IQueryable<TEntity> Query { get; set; }
    private readonly List<string> navigationProperties;

    public EfQuery(IQueryable<TEntity> query, IModel model)
    {
        Query = query;
        navigationProperties = model.FindEntityType(typeof(TEntity))!.GetNavigations().Select(i => i.Name).ToList();
    }

    public void SetIncludes(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes)
    {
        Includes = includes;
    }

    public async override Task<QueryResult<TEntity>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        ApplyJoins();
        ApplyWhere();
        ApplyOrderBy();
        int? totalCount = await ApplyPaginationAsync(cancellationToken);

        var data = await Query.ToListAsync(cancellationToken);
        return new QueryResult<TEntity>
        {
            Data = data,
            ItemsCount = data.Count,
            Page = PaginationContainer?.Page,
            PageSize = PaginationContainer?.PageSize,
            TotalCount = totalCount
        };
    }

    private void ApplyWhere()
    {
        var predicate = ExpressionExtensions.True<TEntity>();
        PredicateContainer.ForEach(i => predicate = i.IsAnd ? predicate.And(i.Expression) : predicate.Or(i.Expression));
        Query = Query.Where(predicate);
    }

    private void ApplyJoins()
    {
        if (JoinContainer.Invoked && !JoinContainer.TableNames.Any())
        {
            navigationProperties.ForEach(i => Query = Query.Include(i));
        }
        JoinContainer.TableNames?.ForEach(i => Query = Query.Include(i));

        if (Includes is not null)
        {
            Query = Includes(Query);
        }
    }

    private async Task<int?> ApplyPaginationAsync(CancellationToken cancellationToken)
    {
        if (PaginationContainer is null)
        {
            return null;
        }

        int? totalCount = null;
        var page = PaginationContainer.Value.Page;
        var pageSize = PaginationContainer.Value.PageSize;

        if (PaginationContainer.Value.Page <= 0)
        {
            page = 1;
        }

        if (IncludeTotalCount)
        {
            totalCount = await Query.CountAsync(cancellationToken);
        }

        Query = Query.Skip(pageSize * (page - 1)).Take(pageSize);

        return totalCount;
    }

    private void ApplyOrderBy()
    {
        if (OrderByContainer is null)
        {
            return;
        }

        var ascending = OrderByContainer.Value.AscendingOrder;
        if (OrderByContainer.Value.KeySelector is not null)
        {
            var keySelector = OrderByContainer.Value.KeySelector;
            Query = ascending ? Query.OrderBy(keySelector) : Query.OrderByDescending(keySelector);
        }

        if (OrderByContainer.Value.PropertyName is not null)
        {
            Query = Query.OrderBy(OrderByContainer.Value.PropertyName, ascending);
        }
    }
}

public class EfQuery<TEntity, TKey> : EfQuery<TEntity>
    where TEntity : Entity<TKey>
    where TKey : IEquatable<TKey>
{
    public EfQuery(IRepository<TEntity, TKey> repository, IModel model) : base(repository.GetQueryable(), model)
    {
    }
}