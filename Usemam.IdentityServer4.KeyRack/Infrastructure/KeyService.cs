using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    public class KeyService : IKeyService
    {
        private readonly KeyRackOptions _options;
        private readonly IKeyRepository _repository;
        private readonly IKeySerializer _serializer;
        private readonly ITimeKeeper _timeKeeper;

        public KeyService(
            KeyRackOptions options,
            IKeyRepository repository,
            IKeySerializer serializer,
            ITimeKeeper timeKeeper)
        {
            _options = options;
            _repository = repository;
            _serializer = serializer;
            _timeKeeper = timeKeeper;
        }

        public async Task<IEnumerable<RsaKey>> GetAllKeysAsync()
        {
            var processTuple = await ProcessKeysAsync();
            return processTuple.Item1;
        }

        public async Task<RsaKey> GetCurrentKeyAsync()
        {
            var processTuple = await ProcessKeysAsync();
            return processTuple.Item2;
        }

        private async Task<(IEnumerable<RsaKey>, RsaKey)> ProcessKeysAsync()
        {
            var keys = await LoadKeysAsync();
            return (keys, GetActiveKey(keys));
        }

        private RsaKey GetActiveKey(IEnumerable<RsaKey> keys)
        {
            if (keys == null || !keys.Any())
            {
                return null;
            }

            var activeKey = GetActiveKeyUseDelay(keys);
            if (activeKey == null)
            {
                activeKey = GetActiveKeyUseDelay(keys, useActivationDelay: false);
            }

            return activeKey;
        }

        private RsaKey GetActiveKeyUseDelay(IEnumerable<RsaKey> keys, bool useActivationDelay = true)
        {
            if (keys == null)
            {
                return null;
            }

            var activeKeys = keys.Where(key => _timeKeeper.IsActive(key, useActivationDelay)).ToArray();
            return activeKeys.Any() ? activeKeys.OrderBy(key => key.Created).First() : null;
        }

        private async Task<IEnumerable<RsaKey>> LoadKeysAsync()
        {
            var storedKeys = await _repository.LoadKeysAsync();
            IEnumerable<RsaKey> keys = storedKeys.Select(_serializer.Deserialize).Where(x => x != null).ToArray();
            return await DeleteRetiredKeysAsync(keys);
        }

        private async Task<IEnumerable<RsaKey>> DeleteRetiredKeysAsync(IEnumerable<RsaKey> keys)
        {
            var retiredKeys = keys.Where(_timeKeeper.IsRetired).ToArray();
            foreach (var retiredKey in retiredKeys)
            {
                await _repository.DeleteKeyAsync(retiredKey.KeyId);
            }

            return keys.Except(retiredKeys).ToArray();
        }
    }
}