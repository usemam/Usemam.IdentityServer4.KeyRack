using Usemam.IdentityServer4.KeyRack;
using Usemam.IdentityServer4.KeyRack.DataProtection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KeyRackBuilderExtensions
    {
        /// <summary>Encrypt serialized keys using <see cref="Microsoft.AspNetCore.DataProtection" /></summary>
        public static KeyRackBuilder AddDataProtection(this KeyRackBuilder builder)
        {
            builder.Services.AddTransient<IKeySerializer, DataProtectionKeySerializer>();
            return builder;
        }
    }
}