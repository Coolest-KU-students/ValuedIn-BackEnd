namespace ValuedInBE.DataControls.Memory
{
    public interface IMemoizationEngine : IDictionary<MemoizationKey, MemoizedValue>
    {
        void Memoize<TKey, TValue>(TKey key, TValue value, TimeSpan timeSpan);
        TValue TryGetValue<TKey, TValue>(TKey key);
        void Memoize<TKey, TValue>(TKey key, TValue value);
        void RemoveByKey<TKey>(TKey key);
    }
}
