using System;

using Microsoft.Extensions.DependencyInjection;

using Usemam.IdentityServer4.KeyRack.EntityFramework;

namespace Usemam.IdentityServer4.KeyRack
{
    public static class KeyRackBuilderExtensions
    {
        public static KeyRackBuilder AddDatabasePersistence(this KeyRackBuilder builder, DatabaseOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.DbContextConfigurationCallback == null)
            {
                throw new ArgumentException("DbContextConfigurationCallback must be configured.");
            }
    
            builder.AddPersistence<EntityFrameworkKeyRepository>();
            builder.Services.AddDbContext<KeyDbContext>(options.DbContextConfigurationCallback);

            return builder;
        }
    }
}