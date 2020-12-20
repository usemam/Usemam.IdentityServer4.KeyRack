using System;
using IdentityServer4.Stores;
using Usemam.IdentityServer4.KeyRack;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KeyRackServiceCollectionExtensions
    {
        public static KeyRackBuilder AddKeyManagement(this IIdentityServerBuilder builder, Action<KeyRackOptions> optionsConfig = null)
        {
            var options = new KeyRackOptions();
            if (optionsConfig != null)
            {
                optionsConfig(options);
            }

            builder.Services.AddSingleton(options);
            builder.Services.AddTransient<IKeySerializer, DefaultKeySerializer>();
            builder.Services.AddTransient<ITimeKeeper, TimeKeeper>();
            builder.Services.AddTransient<IKeyService, KeyService>();
            builder.Services.AddTransient<ISigningCredentialStore, KeyStore>();
            builder.Services.AddTransient<IValidationKeysStore, KeyStore>();

            return new KeyRackBuilder(builder);
        }
    }
}