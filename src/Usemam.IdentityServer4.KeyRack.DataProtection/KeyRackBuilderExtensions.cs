using Microsoft.Extensions.DependencyInjection;

namespace Usemam.IdentityServer4.KeyRack
{
    public static class KeyRackBuilderExtensions
    {
        public static KeyRackBuilder AddDataProtection(this KeyRackBuilder builder)
        {
            builder.Services.AddTransient<IKeySerializer, DataProtection.DataProtectionKeySerializer>();
            return builder;
        }
    }
}