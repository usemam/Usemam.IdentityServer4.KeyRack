using System;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    public interface ITimeKeeper
    {
        DateTime UtcNow { get; }

        bool IsExpired(RsaKey key);

        bool IsRetired(RsaKey key);

        bool IsActive(RsaKey key, bool useActivationDelay);
    }
}