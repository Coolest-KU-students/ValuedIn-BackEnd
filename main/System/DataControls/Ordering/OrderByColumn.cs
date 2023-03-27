using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ValuedInBE.DataControls.Ordering
{
    public class OrderByColumn
    {
        [BindRequired]
        public string Column { get; set; } = string.Empty;
        [BindRequired]
        public bool Ascending { get; set; } = true;

        public IOrderedQueryable<TEntity> ApplyOrderBy<TEntity>(IQueryable<TEntity> query)
        {

            return Ascending
                    ? query.OrderBy(GetProperty<TEntity>())
                    : query.OrderByDescending(GetProperty<TEntity>());
        }

        public IOrderedQueryable<TEntity> ApplyOrderBy<TEntity>(IOrderedQueryable<TEntity> query)
        {
            return Ascending
                    ? query.ThenBy(GetProperty<TEntity>())
                    : query.ThenByDescending(GetProperty<TEntity>());
        }

        private Expression<Func<TEntity, TEntity>> GetProperty<TEntity>() => //I have no idea how I'm able to return TEntity itself and it still works. Might be just using "object" ref under the hood, but best I have for now
                entity => EF.Property<TEntity>(entity!, Column);
    }
}
