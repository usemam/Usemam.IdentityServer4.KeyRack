using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack.EntityFramework
{
    /// <summary><see cref="Microsoft.EntityFrameworkCore" /> implementation of <see cref="IKeyRepository" /></summary>
    public class EntityFrameworkKeyRepository : IKeyRepository
    {
        private readonly KeyDbContext _context;

        private readonly ILogger<EntityFrameworkKeyRepository> _logger;

        public EntityFrameworkKeyRepository(KeyDbContext context, ILogger<EntityFrameworkKeyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc/>
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
                _logger.LogDebug("Concurrency Exception has occured while deleting key {keyId}", keyId);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SerializedKey>> LoadKeysAsync()
        {
            var allKeys = await _context.Keys.ToArrayAsync();
            return allKeys.Select(x =>
                new SerializedKey { KeyId = x.Name, Created = x.Created, Data = x.Value });
        }

        /// <inheritdoc/>
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
