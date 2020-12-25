using Usemam.IdentityServer4.KeyRack;

namespace Usemam.IdentityServer4.KeyRack.IntegrationTests
{
    public interface IKeyServiceIntegrationFactory
    {
        IKeyService CreateService(KeyRackOptions options);
    }
}