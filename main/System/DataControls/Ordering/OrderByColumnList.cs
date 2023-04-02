using Microsoft.Build.Framework;
using System.Linq.Expressions;

namespace ValuedInBE.DataControls.Ordering
{
    public class OrderByColumnList : List<OrderByColumn>
    {
        public IQueryable<TEntity> ApplyOrderingInLinq<TEntity>(IQueryable<TEntity> query)
        {
            Enumerator enumerate = this.GetEnumerator();
            enumerate.MoveNext();
            IOrderedQueryable<TEntity> orderedQuery = enumerate.Current.ApplyOrderBy(query); //First one is against the query, then you stack the others on top

            while (enumerate.MoveNext())
            {
                orderedQuery = enumerate.Current.ApplyOrderBy(orderedQuery);
            }

            return orderedQuery;
        }

        public IQueryable<TEntity> ApplyOrderingInLinq<TEntity>(IQueryable<TEntity> query, CustomColumnMapping<TEntity> customColumnMapping)
        {
            if (!this.Any()) return query;

            IOrderedQueryable<TEntity> orderedQuery;
            Enumerator enumerate = this.GetEnumerator();
            enumerate.MoveNext();
            OrderByColumn currentOrderBy = enumerate.Current;

            //check if there is a custom mapping to be applied to current column
            if (customColumnMapping.TryGetValue(currentOrderBy.Column, out var expression))
            {
                //extracts expression
                Expression<Func<TEntity, object>> func = expression.GetKeySelectorFunction();
                orderedQuery = currentOrderBy.Ascending ? query.OrderBy(func) : query.OrderByDescending(func);
            }
            //if there is no custom mapping, apply normally
            else orderedQuery = currentOrderBy.ApplyOrderBy(query);

            //Proceed with secondary columns if any
            while (enumerate.MoveNext())
            {
                currentOrderBy = enumerate.Current;
                if (customColumnMapping.TryGetValue(currentOrderBy.Column, out expression))
                {
                    Expression<Func<TEntity, object>> func = expression.GetKeySelectorFunction();
                    orderedQuery = currentOrderBy.Ascending ? orderedQuery.ThenBy(func) : orderedQuery.ThenByDescending(func);
                }
                else orderedQuery = currentOrderBy.ApplyOrderBy(orderedQuery);
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