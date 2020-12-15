using System.Collections.Generic;
using System.Threading.Tasks;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    public interface IKeyService
    {
        Task<RsaKey> GetCurrentKeyAsync();

        Task<IEnumerable<RsaKey>> GetAllKeysAsync();
    }
}