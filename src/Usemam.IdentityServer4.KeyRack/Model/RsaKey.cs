using System;
using System.Security.Cryptography;

using Microsoft.IdentityModel.Tokens;

namespace Usemam.IdentityServer4.KeyRack.Model
{
    /// <summary>RSA key wrapper</summary>
    public class RsaKey : Key
    {
        /// <summary>Default constructor</summary>
        public RsaKey()
        {
        }

        /// <summary>Constructor with parameters</summary>
        /// <param name="key">Wrapped key</param>
        /// <param name="created">Key created date</param>
        public RsaKey(RsaSecurityKey key, DateTime created)
            : base(key.KeyId, created)
        {
            RsaParameters = key.Rsa != null
                ? key.Rsa.ExportParameters(includePrivateParameters: true)
                : key.Parameters;
        }

        /// <summary>RSA key parameters</summary>
        public RSAParameters RsaParameters { get; set; }

        /// <summary>Creates <see cref="RsaSecurityKey"/> instance</summary>
        public RsaSecurityKey CreateSecurityKey()
        {
            return new RsaSecurityKey(RsaParameters) { KeyId = KeyId };
        } 
    }
}