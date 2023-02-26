using System.Linq.Expressions;

namespace ValuedInBE.DataControls.Ordering.Internal
{
    public interface IOrderingExpression<TEntity> {

        Type GetKeyType();

        Expression<Func<TEntity, object>> GetKeySelectorFunction();
    
    }

    public class OrderingExpression<TEntity> : IOrderingExpression<TEntity>
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

        public Type GetKeyType()
        {
            return typeof(object);
        }


    }
}
