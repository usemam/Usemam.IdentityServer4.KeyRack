using System;

using Microsoft.AspNetCore.DataProtection;

using Usemam.IdentityServer4.KeyRack;
using Usemam.IdentityServer4.KeyRack.DataProtection;

namespace Usemam.IdentityServer4.KeyRack.IntegrationTests
{
    public class DataProtectionKeyServiceIntegrationFactory : IKeyServiceIntegrationFactory
    {
        private readonly IDataProtectionProvider _dataProtectionProvider =
            new EphemeralDataProtectionProvider();
        private readonly string _keysDirectoryPath = $".keys-{Guid.NewGuid()}";

        public IKeyService CreateService(KeyRackOptions options)
        {
            var timeKeeper = new TimeKeeper(options);
            var serializer = new DataProtectionKeySerializer(_dataProtectionProvider);
            var repository = new FileSystemKeyRepository(_keysDirectoryPath);
            return new KeyService(options, repository, serializer, timeKeeper);
        }
    }
}