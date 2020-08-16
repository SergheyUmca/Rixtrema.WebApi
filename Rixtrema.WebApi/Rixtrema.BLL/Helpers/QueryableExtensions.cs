using System;
using System.Linq;
using System.Linq.Expressions;

namespace Rixtrema.BLL.Helpers
{
    public static class QueryableExtensions
    {
        public enum Order
        {
            Asc,
            Desc
        }

        public static IQueryable<T> OrderByDynamic<T>(
            this IQueryable<T> query,
            string orderByMember,
            Order direction = Order.Asc)
        {
            var queryElementTypeParam = Expression.Parameter(typeof(T));

            Expression memberAccess;
            try
            {
                memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);
            }
            catch (Exception)
            {
                memberAccess = Expression.PropertyOrField(queryElementTypeParam, "Key");
            }
            
            var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction == Order.Asc ? "OrderBy" : "OrderByDescending",
                new[] { typeof(T), memberAccess.Type },
                query.Expression,
                Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderBy);
        }
    }
}