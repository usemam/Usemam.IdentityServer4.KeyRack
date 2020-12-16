using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;

using Moq;

using Xunit;

using Usemam.IdentityServer4.KeyRack;
using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack.UnitTests
{
    public class KeyServiceTests
    {
        private const int KeyInitDelaySec = 1;
        private const int KeyActivationDelaySec = 5;
        private const int KeyExpirationSec = 60;
        private const int KeyRetirementSec = 120;

        private const string RetiredKeyId = "A5C8AA4960F0";
        private const string ExpiredKeyId = "C7A281B5AC6B";
        private const string ActiveKeyId = "AC2345EF097412";
        private const string NewKeyId = "FBA456938E26";

        private KeyService _service;
        private KeyRackOptions _options;
        private ITimeKeeper _timeKeeper;
        private Mock<IKeyRepository> _repositoryMock;
        private Mock<IKeySerializer> _serializerMock;

        public KeyServiceTests()
        {
            _options = CreateTestOptions();
            _timeKeeper = new TimeKeeper(_options);
            _repositoryMock = new Mock<IKeyRepository>();
            _serializerMock = new Mock<IKeySerializer>();

            _service = new KeyService(
                _options,
                _repositoryMock.Object,
                _serializerMock.Object,
                _timeKeeper);
        }

        [Fact]
        public async void SingleActiveKey_ReturnedAsCurrentKey()
        {
            var created = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(KeyActivationDelaySec + 1));
            var key = CreateTestKey(ActiveKeyId, created);
            var serializedKey = new SerializedKey();
            _serializerMock.Setup(x => x.Deserialize(serializedKey)).Returns(key);
            _repositoryMock
                .Setup(x => x.LoadKeysAsync())
                .Returns(Task.FromResult<IEnumerable<SerializedKey>>(new [] {serializedKey}));

            var result = await _service.GetCurrentKeyAsync();

            Assert.NotNull(result);
            Assert.Equal(ActiveKeyId, result.KeyId);
        }

        [Fact]
        public async void NoKeys_NewKeyCreated()
        {
            _repositoryMock
                .Setup(x => x.LoadKeysAsync())
                .Returns(Task.FromResult(Enumerable.Empty<SerializedKey>()));
            var serializedNewKey = new SerializedKey {KeyId = NewKeyId};
            _serializerMock.Setup(x => x.Serialize(It.IsAny<RsaKey>())).Returns(serializedNewKey);

            var result = await _service.GetCurrentKeyAsync();

            _repositoryMock.Verify(
                x => x.StoreKeyAsync(It.Is<SerializedKey>(k => k.KeyId == NewKeyId)), Times.Once);
        }

        [Fact]
        public async void ExpiredKey_NewKeyCreated()
        {
            var created = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(KeyExpirationSec + 1));
            var expiredKey = CreateTestKey(ExpiredKeyId, created);
            var serializedExpiredKey = new SerializedKey {KeyId = ExpiredKeyId};
            _serializerMock.Setup(x => x.Deserialize(serializedExpiredKey)).Returns(expiredKey);

            var serializedNewKey = new SerializedKey {KeyId = NewKeyId};
            _serializerMock
                .Setup(x => x.Serialize(It.Is<RsaKey>(k => k.KeyId != ExpiredKeyId)))
                .Returns(serializedNewKey);

            _repositoryMock
                .Setup(x => x.LoadKeysAsync())
                .Returns(Task.FromResult<IEnumerable<SerializedKey>>(new [] {serializedExpiredKey}));
            
            var result = await _service.GetAllKeysAsync();

            _repositoryMock.Verify(
                x => x.StoreKeyAsync(It.Is<SerializedKey>(k => k.KeyId == NewKeyId)), Times.Once);
        }

        [Fact]
        public async void RetiredKey_Deleted()
        {
            var created = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(KeyRetirementSec + 1));
            var retiredKey = CreateTestKey(RetiredKeyId, created);
            var serializedRetiredKey = new SerializedKey {KeyId = RetiredKeyId};
            _serializerMock.Setup(x => x.Deserialize(serializedRetiredKey)).Returns(retiredKey);

            _repositoryMock
                .Setup(x => x.LoadKeysAsync())
                .Returns(Task.FromResult<IEnumerable<SerializedKey>>(new [] {serializedRetiredKey}));

            var result = await _service.GetAllKeysAsync();

            _repositoryMock.Verify(
                x => x.DeleteKeyAsync(RetiredKeyId), Times.AtLeastOnce);
        }

        private KeyRackOptions CreateTestOptions() =>
            new KeyRackOptions
            {
                KeyInitialization = TimeSpan.FromSeconds(KeyInitDelaySec),
                KeyActivation = TimeSpan.FromSeconds(KeyActivationDelaySec),
                KeyExpiration = TimeSpan.FromSeconds(KeyExpirationSec),
                KeyRetirement = TimeSpan.FromSeconds(KeyRetirementSec)
            };
        
        private RsaKey CreateTestKey(string keyId, DateTime created)
        {
            using (var rsa = RSA.Create())
            {
                var securityKey = new RsaSecurityKey(rsa);
                securityKey.KeyId = keyId;
                return new RsaKey(securityKey, created);
            }
        }
    }
}
