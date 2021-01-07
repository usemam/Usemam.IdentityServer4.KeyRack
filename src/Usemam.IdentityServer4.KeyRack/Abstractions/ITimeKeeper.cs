using System;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    /// <summary>Manages time-related logic</summary>
    public interface ITimeKeeper
    {
        /// <summary>Current UTC date and time</summary>
        DateTime UtcNow { get; }

        /// <summary>Returns <see cref="true"/> if key is expired</summary>
        bool IsExpired(RsaKey key);

        /// <summary>Returns <see cref="true"/> if key is retired</summary>
        bool IsRetired(RsaKey key);

        /// <summary>Returns <see cref="true"/> if key is active</summary>
        /// <param name="key">Key metadata</param>
        /// <param name="useActivationDelay"><see cref="true"/> if key activation period should be considered</param>
        bool IsActive(RsaKey key, bool useActivationDelay);

        /// <summary>Returns key's current age</summary>
        TimeSpan GetKeyAge(RsaKey key);
    }
}