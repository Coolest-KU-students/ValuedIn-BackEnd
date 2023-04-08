namespace ValuedInBE.DataControls.Memory
{
    public abstract class MemoizedValue { }

    public class MemoizedValue<TEntity> : MemoizedValue
    {
        public TEntity? Value { get; init; }
    }
}
