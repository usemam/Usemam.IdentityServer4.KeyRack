using System;
using System.Security.Cryptography;

using Microsoft.IdentityModel.Tokens;

namespace Usemam.IdentityServer4.KeyRack.Model
{
    public class RsaKey : Key
    {
        public RsaKey(RsaSecurityKey key, DateTime created)
            : base(key.KeyId, created)
        {
            RsaParameters = key.Rsa != null
                ? key.Rsa.ExportParameters(includePrivateParameters: true)
                : key.Parameters;
        }

        public RSAParameters RsaParameters { get; }

        public RsaSecurityKey CreateSecurityKey()
        {
            return new RsaSecurityKey(RsaParameters) { KeyId = KeyId };
        } 
    }
}