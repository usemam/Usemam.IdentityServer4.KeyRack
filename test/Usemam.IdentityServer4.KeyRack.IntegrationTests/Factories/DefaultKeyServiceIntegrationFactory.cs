using System;
using System.IO;

namespace Usemam.IdentityServer4.KeyRack.IntegrationTests
{
    public class DefaultKeyServiceIntegrationFactory : IKeyServiceIntegrationFactory, IDisposable
    {
        private readonly string _keysDirectoryPath = $".keys-{Guid.NewGuid()}";

        public IKeyService CreateService(KeyRackOptions options)
        {
            var timeKeeper = new TimeKeeper(options);
            var serializer = new DefaultKeySerializer();
            var repository = new FileSystemKeyRepository(_keysDirectoryPath);
            return new KeyService(options, repository, serializer, timeKeeper);
        }

        public void Dispose()
        {
            Directory.Delete(_keysDirectoryPath, true);
        }
    }
}