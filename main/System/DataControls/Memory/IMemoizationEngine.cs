namespace ValuedInBE.DataControls.Memory
{
    public interface IMemoizationEngine
    {
        void Memoize<TKey, TValue>(TKey key, TValue value, TimeSpan timeSpan);
        void Memoize<TKey, TValue>(TKey key, TValue value);
        void RemoveByKey<TKey>(TKey key);
        TValue? GetValue<TKey, TValue>(TKey key);
        TValue? Extract<TKey, TValue>(TKey key);
    }
}