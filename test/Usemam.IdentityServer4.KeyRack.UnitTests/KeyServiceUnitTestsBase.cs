using System;
using System.Security.Cryptography;

using Microsoft.IdentityModel.Tokens;
using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack.UnitTests
{
    public class KeyServiceUnitTestsBase
    {
        protected const int KeyInitDelaySec = 1;
        protected const int KeyActivationDelaySec = 5;
        protected const int KeyExpirationSec = 60;
        protected const int KeyRetirementSec = 120;

        protected const string RetiredKeyId = "A5C8AA4960F0";
        protected const string ExpiredKeyId = "C7A281B5AC6B";
        protected const string ActiveKeyId = "AC2345EF097412";
        protected const string NewKeyId = "FBA456938E26";

        protected RsaKey CreateTestKey(string keyId, DateTime created)
        {
            using (var rsa = RSA.Create())
            {
                var securityKey = new RsaSecurityKey(rsa);
                securityKey.KeyId = keyId;
                return new RsaKey(securityKey, created);
            }
        }

        protected KeyRackOptions CreateTestOptions() =>
            new KeyRackOptions
            {
                KeyInitialization = TimeSpan.FromSeconds(KeyInitDelaySec),
                KeyActivation = TimeSpan.FromSeconds(KeyActivationDelaySec),
                KeyExpiration = TimeSpan.FromSeconds(KeyExpirationSec),
                KeyRetirement = TimeSpan.FromSeconds(KeyRetirementSec)
            };
    }
}
