using JetBrains.Annotations;
using System.Linq.Expressions;

namespace BU.OnlineShop.CatalogService.Database.Extensions
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

    }
}
