using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace ValuedInBE.DataControls.Memory
{
    public class MemoizationEngine : IMemoizationEngine
    {
        private readonly ConcurrentDictionary<MemoizationKey, MemoizedValue> _memoized;
        private readonly ILogger<MemoizationEngine> _logger;

        public ImmutableDictionary<MemoizationKey, MemoizedValue> InnerDictionary { get => _memoized.ToImmutableDictionary(); }
        public int Count => _memoized.Count;

        public MemoizationEngine(ILogger<MemoizationEngine> logger)
        {
            _logger = logger;
            _memoized = new();
        }

        public void Memoize<TKey, TValue>(TKey key, TValue value)
        {
            _logger.LogTrace("Memoization with Key({key}) and Value({value})", key, value);
            MemoizationKey<TKey> memoizationKey = MemKey(key);
            MemoizedValue<TValue> memoizedValue = MemValue(value);

            _memoized.TryAdd(memoizationKey, memoizedValue);
        }

        public void Memoize<TKey, TValue>(TKey key, TValue value, TimeSpan timeSpan)
        {
            Memoize(key, value);

            _logger.LogTrace("Memoization with Key({key}) and Value({value}) will expire in {lifeTime} seconds", key, value, timeSpan.TotalSeconds);
            RemoveKeyAfterSpecifiedTime(key, timeSpan); //discard so it does not wait till task executes
        }

        public TValue? GetValue<TKey, TValue>(TKey key)
        {
            _logger.LogDebug("Attempting to find memoized value with Key({key})", key);
            bool exists =  _memoized.TryGetValue(MemKey(key), out MemoizedValue? memoizedValue);

            if (!exists || memoizedValue == null)
            {
                return default;
            }
            _logger.LogTrace("Found memoized value with Key({key}) which will is being cast to {TValue} type", key, typeof(TValue));
            MemoizedValue<TValue> specifiedTypeValue = (MemoizedValue<TValue>)memoizedValue;
            return specifiedTypeValue.Value;
        }

        public TValue? Extract<TKey, TValue>(TKey key)
        {
            _logger.LogTrace("Attempting to remove a memoized value with Key({key})", key);
            if (_memoized.TryRemove(MemKey(key), out MemoizedValue? value) && value != null)
            {
                MemoizedValue<TValue> specifiedTypeValue = (MemoizedValue<TValue>)value;
                return specifiedTypeValue.Value;
            }
            return default;
        }

        public void RemoveByKey<TKey>(TKey key)
        {
            _logger.LogTrace("Attempting to remove a memoized value with Key({key})", key);
            _memoized.TryRemove(MemKey(key), out _);
        }


        private async void RemoveKeyAfterSpecifiedTime<TKey>(TKey key, TimeSpan lifeTime)
        {
            await Task.Delay(lifeTime);
            _logger.LogTrace("Attempting to remove a memoized value with Key({key})", key);
            this.RemoveByKey(key);
        }

        private static MemoizationKey<TKey> MemKey<TKey>(TKey key)
        {
            return new() { Key = key };
        }

        private static MemoizedValue<TValue> MemValue<TValue>(TValue value)
        {
            return new() { Value = value };
        }
    }
}
