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
            if (key == null)
            {
                return false;
            }

            var time1 = UtcNow;
            var time2 = key.Created;
            if (useActivationDelay)
            {
                time2 = time2.Add(_options.KeyActivation);
            }

            if (time2 > time1)
            {
                // key not yet active
                return false;
            }

            return !IsExpired(key);
        }

        public bool IsExpired(RsaKey key) => GetKeyAge(key) >= _options.KeyExpiration;

        public bool IsRetired(RsaKey key) => GetKeyAge(key) >= _options.KeyRetirement;

        public TimeSpan GetKeyAge(RsaKey key) => UtcNow - key.Created;
    }
}