﻿using System.Linq.Expressions;

namespace ValuedInBE.System.DataControls.Ordering
{
    public class OrderingExpression<TEntity>
    {
        private readonly Expression<Func<TEntity, object>> _func;

        public OrderingExpression(Expression<Func<TEntity, object>> getKeyFunction)
        {
            _func = getKeyFunction;
        }

        public Expression<Func<TEntity, object>> GetKeySelectorFunction()
        {
            return _func;
        }
    }
}
