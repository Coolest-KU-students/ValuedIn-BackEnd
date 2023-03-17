namespace ValuedInBE.DataControls.Memory
{
    public interface IMemoizationEngine
    {
        void Memoize<TKey, TValue>(TKey key, TValue value, TimeSpan timeSpan);
        TValue TryGetValue<TKey, TValue>(TKey key);
        void Memoize<TKey, TValue>(TKey key, TValue value);
        void RemoveByKey<TKey>(TKey key);
        TValue TryGetAndRemove<TKey, TValue>(TKey key);
    }
}
