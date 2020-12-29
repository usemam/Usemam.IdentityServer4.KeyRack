using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack.EntityFramework
{
    public class EntityFrameworkKeyRepository : IKeyRepository
    {
        private readonly KeyDbContext _context;

        public EntityFrameworkKeyRepository(KeyDbContext context) => _context = context;

        public async Task DeleteKeyAsync(string keyId)
        {
            var key = await _context.Keys.FirstOrDefaultAsync(x => x.Name == keyId);
            if (key == null)
            {
                return;
            }

            try
            {
                _context.Keys.Remove(key);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                _context.Entry(key).State = EntityState.Detached;
            }
        }

        public async Task<IEnumerable<SerializedKey>> LoadKeysAsync()
        {
            var allKeys = await _context.Keys.ToArrayAsync();
            return allKeys.Select(x =>
                new SerializedKey { KeyId = x.Name, Created = x.Created, Data = x.Value });
        }

        public Task StoreKeyAsync(SerializedKey key)
        {
            var entity = new Key
            {
                Name = key.KeyId,
                Created = key.Created,
                Value = key.Data
            };
            _context.Keys.Add(entity);
            return _context.SaveChangesAsync();
        }
    }
}
