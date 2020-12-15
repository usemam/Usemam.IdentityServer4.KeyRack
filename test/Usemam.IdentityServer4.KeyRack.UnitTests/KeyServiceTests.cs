using System;
using System.Collections.Generic;
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

        private const string ActiveKeyId = "AC2345EF097412";

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
