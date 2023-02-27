namespace ValuedInBE.DataControls.Memory
{
    public abstract class MemoizationKey
    {
    }

    public class MemoizationKey<TKey> : MemoizationKey
    {
        public TKey Key { get; init; }

        public override bool Equals(object obj)
        {
            return obj is MemoizationKey<TKey> other &&
                   Key.Equals(other.Key);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
