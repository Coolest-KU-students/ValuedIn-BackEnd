using System.Collections.Concurrent;

namespace ValuedInBE.DataControls.Memory
{
    public class MemoizationEngine : ConcurrentDictionary<MemoizationKey, MemoizedValue>, IMemoizationEngine
    {
        public void Memoize<TKey, TValue>(TKey key, TValue value)
        {
            MemoizationKey<TKey> memoizationKey = new() { Key = key };
            MemoizedValue<TValue> memoizedValue = new() { Value = value };

            TryAdd(memoizationKey, memoizedValue);
        }

        public void Memoize<TKey, TValue>(TKey key, TValue value, TimeSpan lifeTime)
        {
            Memoize(key, value);
            _ = RemoveKeyAfterSpecifiedTime(key, lifeTime); //discard so it does not wait till task executes
        }

        public TValue TryGetValue<TKey, TValue>(TKey key)
        {
            bool exists = this.TryGetValue(new MemoizationKey<TKey>() { Key = key }, out MemoizedValue memoizedValue);

            if (!exists)
            {
                throw new KeyNotFoundException();
            }

            MemoizedValue<TValue> specifiedTypeValue = (MemoizedValue<TValue>) memoizedValue;
            return specifiedTypeValue.Value;
        }

        public void RemoveByKey<TKey>(TKey key)
        {
            TryRemove(new MemoizationKey<TKey>() { Key = key }, out _);
        }

        private async Task RemoveKeyAfterSpecifiedTime<TKey>(TKey key, TimeSpan lifeTime)
        {
            await Task.Delay(lifeTime);
            this.RemoveByKey(key);
        }
    }

    
}
