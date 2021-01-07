using System;

namespace Usemam.IdentityServer4.KeyRack
{
    public class KeyRackOptions
    {
        /// <summary>Key identifier size, in bits. Defaults to 256.</summary>
        public int KeyIdSize { get; set; } = 256;
        
        /// <summary>Key size, in bits. Defaults to 2048.</summary>
        public int KeySize { get; set; } = 2048;

        /// <summary>
        /// The age of the key after which it is considered to be "expired".
        /// Expired key is not used for signing, but used for validation.
        /// Defaults to 90 days.
        /// </summary>
        public TimeSpan KeyExpiration { get; set; } = TimeSpan.FromDays(90);

        /// <summary>
        /// The age of the key after which it is considered to be "retired".
        /// Retired key is deleted after this time period.
        /// Defaults to 180 days.
        /// </summary>
        public TimeSpan KeyRetirement { get; set; } = TimeSpan.FromDays(180);

        /// <summary>
        /// Time period during which new key is discovered by all nodes.
        /// Defaults to 7 days.
        /// </summary>
        public TimeSpan KeyActivation { get; set; } = TimeSpan.FromDays(7);

        /// <summary>
        /// New key initialization delay.
        /// This time period is used to allow all nodes to sync when keys are created first time.
        /// Defaults to 5 seconds.
        /// </summary>
        public TimeSpan KeyInitialization { get; set; } = TimeSpan.FromSeconds(5);
    }
}