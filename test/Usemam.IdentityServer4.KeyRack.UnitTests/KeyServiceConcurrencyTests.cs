using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack.UnitTests
{
    public class KeyServiceConcurrencyTests : KeyServiceUnitTestsBase
    {
        private const int ServiceExecutionTimeout = 5 * 1000; // 5 seconds
        private KeyRackOptions _options;
        private ITimeKeeper _timeKeeper;
        private IKeySerializer _serializer;
        private IKeyRepository _repository;

        public KeyServiceConcurrencyTests()
        {
            _options = CreateTestOptions();
            _timeKeeper = new TimeKeeper(_options);
            _serializer = new DefaultKeySerializer();
        }

        [Fact]
        public async void NoKeys_NewKeysCreated()
        {
            var storedKeys = await RunConcurrentServices(Enumerable.Empty<SerializedKey>());

            Assert.NotNull(storedKeys);
            Assert.True(storedKeys.Count() >= 1);
        }

        [Fact]
        public async void RetiredKey_NewKeysCreated()
        {
            var retiredKey = CreateTestKey(RetiredKeyId, DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(KeyRetirementSec + 1)));
            var retiredKeySerialized = _serializer.Serialize(retiredKey);
            var storedKeys = await RunConcurrentServices(new[] { retiredKeySerialized });
            
            Assert.NotNull(storedKeys);
            Assert.True(storedKeys.Where(x => x != retiredKeySerialized).Count() >= 1);
        }

        [Fact]
        public async void ExpiredKey_NewKeysCreated()
        {
            var expiredKey = CreateTestKey(RetiredKeyId, DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(KeyExpirationSec + 1)));
            var expiredKeySerialized = _serializer.Serialize(expiredKey);
            var storedKeys = await RunConcurrentServices(new[] { expiredKeySerialized });
            
            Assert.NotNull(storedKeys);
            Assert.True(storedKeys.Where(x => x != expiredKeySerialized).Count() >= 1);
        }

        [Fact]
        public void NoKeys_SameKeyReturnedAsActive()
        {
            _repository = new TestKeyRepository();
            var service1 = NewService();
            var service2 = NewService();

            RsaKey key1 = null;
            RsaKey key2 = null;

            Task.WaitAll(
                new[] {
                    Task.Run(() => key1 = service1.GetCurrentKeyAsync().Result),
                    Task.Run(() => key2 = service2.GetCurrentKeyAsync().Result)
                },
                ServiceExecutionTimeout);
            
            Assert.NotNull(key1);
            Assert.NotNull(key2);
            Assert.Equal(key1, key2);
        }

        [Fact]
        public async void RetiredKey_RetiredKeyDeleted()
        {
            var retiredKey = CreateTestKey(RetiredKeyId, DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(KeyRetirementSec + 1)));
            var storedKeys = await RunConcurrentServices(new[] { _serializer.Serialize(retiredKey) });

            Assert.NotNull(storedKeys);
            Assert.Null(storedKeys.FirstOrDefault(x => x.KeyId == ExpiredKeyId));
        }

        private async Task<IEnumerable<SerializedKey>> RunConcurrentServices(IEnumerable<SerializedKey> existingKeys)
        {
            _repository = new TestKeyRepository(existingKeys);

            var service1 = NewService();
            var service2 = NewService();

            Task.WaitAll(
                Task.Run(() => service1.GetAllKeysAsync().Wait(ServiceExecutionTimeout)),
                Task.Run(() => service2.GetAllKeysAsync().Wait(ServiceExecutionTimeout)));

            return await _repository.LoadKeysAsync();
        }

        private IKeyService NewService() =>
            new KeyService(_options, _repository, _serializer, _timeKeeper, new Mock<ILogger<KeyService>>().Object);

        private class TestKeyRepository : IKeyRepository
        {
            private ConcurrentDictionary<string, SerializedKey> _store;
            
            public TestKeyRepository()
            {
                _store = new ConcurrentDictionary<string, SerializedKey>();
            }

            public TestKeyRepository(IEnumerable<SerializedKey> keys)
            {
                _store = new ConcurrentDictionary<string, SerializedKey>();
                foreach (var key in keys)
                {
                    _store.TryAdd(key.KeyId, key);
                }
            }

            public Task DeleteKeyAsync(string keyId)
            {
                _store.TryRemove(keyId, out _);
                return Task.CompletedTask;
            }

            public Task<IEnumerable<SerializedKey>> LoadKeysAsync()
            {
                return Task.FromResult((IEnumerable<SerializedKey>) _store.Values);
            }

            public Task StoreKeyAsync(SerializedKey key)
            {
                _store.TryAdd(key.KeyId, key);
                return Task.CompletedTask;
            }
        }
    }
}