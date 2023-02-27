using System.Collections.Concurrent;

namespace ValuedInBE.DataControls.Memory
{
    public class MemoizationEngine : ConcurrentDictionary<MemoizationKey, MemoizedValue>, IMemoizationEngine
    {

        private readonly ILogger<MemoizationEngine> _logger;

        public MemoizationEngine(ILogger<MemoizationEngine> logger)
        {
            _logger = logger;
        }

        public void Memoize<TKey, TValue>(TKey key, TValue value)
        {
            _logger.LogTrace("Memoization with Key({key}) and Value({value})", key, value);
            MemoizationKey<TKey> memoizationKey = new() { Key = key };
            MemoizedValue<TValue> memoizedValue = new() { Value = value };

            TryAdd(memoizationKey, memoizedValue);
        }

        public void Memoize<TKey, TValue>(TKey key, TValue value, TimeSpan lifeTime)
        {
            Memoize(key, value);

            _logger.LogTrace("Memoization with Key({key}) and Value({value}) will expire in {lifeTime} seconds", key, value, lifeTime.TotalSeconds);
            _ = RemoveKeyAfterSpecifiedTime(key, lifeTime); //discard so it does not wait till task executes
        }

        public TValue TryGetValue<TKey, TValue>(TKey key)
        {
            _logger.LogDebug("Attempting to find memoized value with Key({key})", key);
            bool exists = this.TryGetValue(new MemoizationKey<TKey>() { Key = key }, out MemoizedValue memoizedValue);

            if (!exists)
            {
                throw new KeyNotFoundException();
            }
            _logger.LogTrace("Found memoized value with Key({key}) which will is being cast to {TValue} type", key, typeof(TValue));
            MemoizedValue<TValue> specifiedTypeValue = (MemoizedValue<TValue>)memoizedValue;
            return specifiedTypeValue.Value;
        }

        public void RemoveByKey<TKey>(TKey key)
        {
            _logger.LogTrace("Attempting to remove a memoized value with Key({key})", key);
            TryRemove(new MemoizationKey<TKey>() { Key = key }, out _);
        }

        private async Task RemoveKeyAfterSpecifiedTime<TKey>(TKey key, TimeSpan lifeTime)
        {
            await Task.Delay(lifeTime);
            _logger.LogTrace("Attempting to remove a memoized value with Key({key})", key);
            this.RemoveByKey(key);
        }
    }


}
