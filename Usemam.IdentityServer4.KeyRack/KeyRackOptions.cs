using System;

namespace Usemam.IdentityServer4.KeyRack
{
    public class KeyRackOptions
    {
        public int KeyIdSize { get; set; } = 256;
        
        public int KeySize { get; set; } = 2048;

        public TimeSpan KeyExpiration { get; set; } = TimeSpan.FromDays(90);

        public TimeSpan KeyRetirement { get; set; } = TimeSpan.FromDays(180);

        public TimeSpan KeyActivation { get; set; } = TimeSpan.FromDays(7);
    }
}