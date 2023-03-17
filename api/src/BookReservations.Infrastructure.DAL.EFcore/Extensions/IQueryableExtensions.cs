using System.Linq.Expressions;
using System.Reflection;

namespace BookReservations.Infrastructure.DAL.EFcore.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, bool ascending = true)
        where T : class
    {
        if (!typeof(T).HasProperty(propertyName))
        {
            return query;
        }
        var keySelector = CreateSelector<T>(propertyName);
        return ascending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
    }

    public static Expression<Func<T, object>> CreateSelector<T>(string propertyName)
        where T : class
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);

        return Expression.Lambda<Func<T, object>>(
            Expression.Convert(property, typeof(object)), parameter);
    }

    public static bool HasProperty(this Type type, string propertyName)
    {
        var property = type.GetProperty(
            propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        return property is not null;
    }
}
