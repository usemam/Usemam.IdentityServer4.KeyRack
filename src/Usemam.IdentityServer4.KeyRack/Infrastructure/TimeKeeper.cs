using System;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    /// <summary>Default <see cref="ITimeKeeper" /> implementation</summary>
    public class TimeKeeper : ITimeKeeper
    {
        private readonly KeyRackOptions _options;

        public TimeKeeper(KeyRackOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public DateTime UtcNow => DateTime.UtcNow;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public bool IsExpired(RsaKey key) => GetKeyAge(key) >= _options.KeyExpiration;

        /// <inheritdoc />
        public bool IsRetired(RsaKey key) => GetKeyAge(key) >= _options.KeyRetirement;

        /// <inheritdoc />
        public TimeSpan GetKeyAge(RsaKey key) => UtcNow - key.Created;
    }
}