using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ValuedInBE.DataControls.Ordering
{
    public class OrderByColumnList : List<OrderByColumn>
    {
        public IQueryable<TEntity> ApplyOrderingInLinq<TEntity>(IQueryable<TEntity> query)
        {
            Enumerator enumerate = this.GetEnumerator();
            IOrderedQueryable<TEntity> orderedQuery = enumerate.Current.ApplyOrderBy(query); //First one is against the query, then you stack the others on top

            while (enumerate.MoveNext())
            {
                orderedQuery = enumerate.Current.ApplyOrderBy(orderedQuery);
            }

            return orderedQuery;
        }

        public IQueryable<TEntity> ApplyOrderingInLinq<TEntity>(IQueryable<TEntity> query, CustomColumnMapping<TEntity> customColumnMapping)
        {
            if (Count == 0) return query;

            Enumerator enumerate = this.GetEnumerator();
            OrderByColumn orderBy = enumerate.Current;
            IOrderedQueryable<TEntity> orderedQuery;

            if (customColumnMapping.TryGetValue(orderBy.Column, out var expression)) {
                Expression<Func<TEntity, object>> func = expression.GetKeySelectorFunction();
                orderedQuery = orderBy.Ascending ? query.OrderBy(func) : query.OrderByDescending(func);
            }
            else orderedQuery = orderBy.ApplyOrderBy(query);

            while (enumerate.MoveNext())
            {
                orderBy = enumerate.Current;
                if (customColumnMapping.TryGetValue(orderBy.Column, out expression))
                {
                    Expression<Func<TEntity, object>> func = expression.GetKeySelectorFunction();
                    orderedQuery = orderBy.Ascending ? orderedQuery.ThenBy(func) : orderedQuery.ThenByDescending(func);
                }
                else orderedQuery = orderBy.ApplyOrderBy(orderedQuery);
            }
            return orderedQuery;
        }
    }

    public static class OrderByExtensions
    {
        public static IQueryable<TEntity> ApplyOrderingInLinq<TEntity>(this IQueryable<TEntity> query, OrderByColumnList orderByColumns)
        {
            return orderByColumns.ApplyOrderingInLinq(query);
        }

        public static IQueryable<TEntity> ApplyOrderingInLinq<TEntity>(this IQueryable<TEntity> query, OrderByColumnList orderByColumns, CustomColumnMapping<TEntity> customColumnMapping)
        {
            return orderByColumns.ApplyOrderingInLinq(query, customColumnMapping);
        }
    }
}