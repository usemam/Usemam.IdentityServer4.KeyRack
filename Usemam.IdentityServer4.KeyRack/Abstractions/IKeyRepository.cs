using System.Collections.Generic;
using System.Threading.Tasks;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    public interface IKeyRepository
    {
        Task<IEnumerable<SerializedKey>> LoadKeysAsync();

        Task StoreKeyAsync(SerializedKey key);

        Task DeleteKeyAsync(string keyId);
    }
}