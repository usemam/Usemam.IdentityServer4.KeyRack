using System;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    public class TimeKeeper : ITimeKeeper
    {
        private readonly KeyRackOptions _options;

        public TimeKeeper(KeyRackOptions options)
        {
            _options = options;
        }

        public DateTime UtcNow => DateTime.UtcNow;

        public bool IsActive(RsaKey key, bool useActivationDelay)
        {
            throw new NotImplementedException();
        }

        public bool IsExpired(RsaKey key) => GetKeyAge(key) >= _options.KeyExpiration;

        public bool IsRetired(RsaKey key) => GetKeyAge(key) >= _options.KeyRetirement;

        private TimeSpan GetKeyAge(RsaKey key) => UtcNow - key.Created;
    }
}