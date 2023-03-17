using BookReservations.Infrastructure.DAL.EFcore.Query;
using BookReservations.Infrastructure.DAL.Query.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BookReservations.Infrastructure.DAL.EFcore.Extensions;

public static class QueryExtensions
{
    public static IAfterJoinQuery<TEntity> Join<TEntity>(this IQuery<TEntity> query, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes)
        where TEntity : class, IBaseEntity
    {
        if (query is not EfQuery<TEntity> efQuery)
        {
            throw new ApplicationException("This method can not be used on non-EfCore query class");
        }

        efQuery.SetIncludes(includes);
        return efQuery;
    }

    public static IAfterJoinQuery<TEntity> Join<TEntity>(this IQuery<TEntity> query, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes)
    where TEntity : class, IBaseEntity
    {
        if (query is not EfQuery<TEntity> efQuery)
        {
            throw new ApplicationException("This method can not be used on non-EfCore query class");
        }

        efQuery.SetIncludes(includes);
        return efQuery;
    }

    public static IAfterWhereQuery<TEntity> WhereIfNotNull<TEntity>(this IQuery<TEntity> query, Expression<Func<TEntity, bool>>? predicate)
        where TEntity : class, IBaseEntity
    {
        if (predicate is null)
        {
            return query.Where(_ => true);
        }
        return query.Where(predicate);
    }

    public static IAfterWhereQuery<TEntity> WhereIfNotNull<TEntity>(this IAfterJoinQuery<TEntity> query, Expression<Func<TEntity, bool>>? predicate)
        where TEntity : class, IBaseEntity
    {
        if (predicate is null)
        {
            return query.Where(_ => true);
        }
        return query.Where(predicate);
    }
}
