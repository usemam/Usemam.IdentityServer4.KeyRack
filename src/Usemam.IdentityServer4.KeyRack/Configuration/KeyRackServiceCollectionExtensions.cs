using System;
using IdentityServer4.Stores;
using Usemam.IdentityServer4.KeyRack;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class KeyRackServiceCollectionExtensions
    {
        /// <summary>
        /// Enables key management add-on for IdentityServer
        /// </summary>
        /// <param name="optionsConfig">Custom configuration options</param>
        /// <returns><see cref="KeyRackBuilder" /> instance</returns>
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