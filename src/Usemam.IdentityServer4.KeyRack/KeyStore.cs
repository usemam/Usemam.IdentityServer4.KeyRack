using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.IdentityModel.Tokens;

namespace Usemam.IdentityServer4.KeyRack
{
    /// <summary>
    /// <see cref="ISigningCredentialStore"/> and <see cref="IValidationKeysStore"/> implementation using <see cref="KeyService"/>
    /// </summary>
    public class KeyStore : ISigningCredentialStore, IValidationKeysStore
    {
        private readonly IKeyService _keyService;

        private readonly KeyRackOptions _options;

        public KeyStore(IKeyService keyService, KeyRackOptions options)
        {
            _keyService = keyService;
            _options = options;
        }

        async Task<SigningCredentials> ISigningCredentialStore.GetSigningCredentialsAsync()
        {
            var key = await _keyService.GetCurrentKeyAsync();
            return new SigningCredentials(key.CreateSecurityKey(), SigningAlgorithm);
        }

        async Task<IEnumerable<SecurityKeyInfo>> IValidationKeysStore.GetValidationKeysAsync()
        {
            var keys = await _keyService.GetAllKeysAsync();
            return keys
                .Select(key => key.CreateSecurityKey())
                .Select(key => new SecurityKeyInfo { Key = key, SigningAlgorithm = SigningAlgorithm });
        }

        /// <summary>Signing algorithm defaults to RS256 for now.</summary>
        private string SigningAlgorithm => IdentityServerConstants.RsaSigningAlgorithm.RS256.ToString();
    }
}
