using Usemam.IdentityServer4.KeyRack;

namespace Usemam.IdentityServer4.KeyRack.IntegrationTests
{
    public class DefaultKeyServiceIntegrationFactory : IKeyServiceIntegrationFactory
    {
        public IKeyService CreateService(KeyRackOptions options)
        {
            var timeKeeper = new TimeKeeper(options);
            var serializer = new DefaultKeySerializer();
            var repository = new FileSystemKeyRepository(".keys");
            return new KeyService(options, repository, serializer, timeKeeper);
        }
    }
}