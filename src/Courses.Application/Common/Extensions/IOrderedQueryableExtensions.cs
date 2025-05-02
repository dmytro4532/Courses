using System.Linq.Expressions;

namespace Courses.Application.Common.Extensions;

public static class IQueryableExtensions
{
    public static IOrderedQueryable<TSource> GetOrderedQuery<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, string orderDirection)
    {
        return string.Equals(orderDirection, "ASC", StringComparison.OrdinalIgnoreCase)
            ? source.OrderBy(keySelector)
            : source.OrderByDescending(keySelector);
    }
}
