using System;
using System.Linq.Expressions;
using ValuedInBE.DataControls.Ordering.Internal;

namespace ValuedInBE.DataControls.Ordering
{
    public class CustomColumnMapping<TEntity> : Dictionary<string, OrderingExpression<TEntity>>
    {
    }
}
