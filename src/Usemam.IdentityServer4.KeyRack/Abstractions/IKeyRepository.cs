using System.Collections.Generic;
using System.Threading.Tasks;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    /// <summary>Key persistence abstraction</summary>
    public interface IKeyRepository
    {
        /// <summary>Loads all available keys</summary>
        Task<IEnumerable<SerializedKey>> LoadKeysAsync();

        /// <summary>Stores new key</summary>
        Task StoreKeyAsync(SerializedKey key);

        /// <summary>Deletes key by id</summary>
        Task DeleteKeyAsync(string keyId);
    }
}