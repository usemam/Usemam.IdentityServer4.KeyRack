using Microsoft.Extensions.DependencyInjection;

namespace Usemam.IdentityServer4.KeyRack
{
    public static class KeyRackBuilderExtensions
    {
        /// <summary>Encrypt serialized keys using <see cref="Microsoft.AspNetCore.DataProtection" /></summary>
        public static KeyRackBuilder AddDataProtection(this KeyRackBuilder builder)
        {
            builder.Services.AddTransient<IKeySerializer, DataProtection.DataProtectionKeySerializer>();
            return builder;
        }
    }
}