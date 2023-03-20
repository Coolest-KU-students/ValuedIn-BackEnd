using Microsoft.Extensions.Logging;
using Moq;
using ValuedInBE.DataControls.Memory;
using Xunit;

namespace ValuedInBETests.System.DataControls.Memory
{
    public class MemoizationEngineTests
    {
        private readonly MemoizationEngine _memoizationEngine;
        private readonly Mock<ILogger<MemoizationEngine>> _logger = new();
        private readonly List<string> _listAsKey = new();
        private readonly Func<string, string> _functionThatReturnsTheInput = something => something;
        private const string testingValue = "test";
        private const int stressTestAmount = 50000;
        private const int stressTestAsyncTimePillow = 100;
        private readonly TimeSpan _timeout = TimeSpan.FromMilliseconds(5000);


        public MemoizationEngineTests()
        {
            _memoizationEngine = new(_logger.Object);
        }

        [Fact]
        public async Task AddWithTimeOutAndThenRetrieveAndThenCheckIfDeletedAfterTimeOut()
        {
            _memoizationEngine.Memoize(_listAsKey, _functionThatReturnsTheInput, _timeout);

            Assert.Single(_memoizationEngine);
            Func<string, string> retrieved =
                _memoizationEngine.TryGetValue<List<string>, Func<string, string>>(_listAsKey);

            Assert.Equal(testingValue, retrieved.Invoke(testingValue));

            await Task.Delay(_timeout);
            await Task.Delay(stressTestAsyncTimePillow);

            Assert.Empty(_memoizationEngine);
            Assert.Throws<KeyNotFoundException>(
                () => _memoizationEngine.TryGetValue<List<string>, Func<string, string>>(_listAsKey)
             );
        }

        [Fact]
        public void AddWithoutTimeoutAndRemove()
        {
            _memoizationEngine.Memoize(_listAsKey, _functionThatReturnsTheInput);
            Assert.Single(_memoizationEngine);

            _memoizationEngine.RemoveByKey(_listAsKey);
            Assert.Empty(_memoizationEngine);
            Assert.Throws<KeyNotFoundException>(
                () => _memoizationEngine.TryGetValue<List<string>, Func<string, string>>(_listAsKey)
             );
        }

        [Fact]
        public async Task StressTestingRemovalOperationsAsync()
        {
            foreach (int number in Enumerable.Range(0, stressTestAmount))
            {
                _memoizationEngine.Memoize(number, testingValue, _timeout);
            }

            Assert.True(_memoizationEngine.Count == stressTestAmount);

            await Task.Delay(_timeout);
            await Task.Delay(stressTestAsyncTimePillow);
            Assert.Empty(_memoizationEngine);
            Assert.Throws<KeyNotFoundException>(
                () => _memoizationEngine.TryGetValue<List<string>, Func<string, string>>(_listAsKey)
             );
        }
    }
}