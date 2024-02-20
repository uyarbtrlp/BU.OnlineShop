using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BU.OnlineShop.Shared.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return condition
                ? query.Where(predicate)
                : query;
        }

        public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }

        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return condition
                ? query.Where(predicate)
                : query;
        }

        public static TQueryable WhereIf<T, TQueryable>([NotNull] this TQueryable query, bool condition, Expression<Func<T, int, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return condition
                ? (TQueryable)query.Where(predicate)
                : query;
        }

        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query;
        }

    }
}
