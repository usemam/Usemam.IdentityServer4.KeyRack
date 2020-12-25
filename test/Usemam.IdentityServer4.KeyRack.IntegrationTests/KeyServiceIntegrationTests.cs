using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

using Usemam.IdentityServer4.KeyRack;
using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack.IntegrationTests
{
    public class KeyServiceIntegrationTests
    {
        private KeyRackOptions CreateTestOptions(
            int keyInitDelaySec = 1,
            int keyActivationDelaySec = 5,
            int keyExpirationSec = 60,
            int keyRetirementSec = 120) =>
            new KeyRackOptions
            {
                KeyInitialization = TimeSpan.FromSeconds(keyInitDelaySec),
                KeyActivation = TimeSpan.FromSeconds(keyActivationDelaySec),
                KeyExpiration = TimeSpan.FromSeconds(keyExpirationSec),
                KeyRetirement = TimeSpan.FromSeconds(keyRetirementSec)
            };

        [Theory]
        [ClassData(typeof(KeyServiceFactoryData))]
        public void NoKeys_SameKeyReturnedAsActive(IKeyServiceIntegrationFactory factory)
        {
            var options = CreateTestOptions();
            var service1 = factory.CreateService(options);
            var service2 = factory.CreateService(options);

            RsaKey key1 = null;
            RsaKey key2 = null;

            Task.WaitAll(
                new[] {
                    Task.Run(() => key1 = service1.GetCurrentKeyAsync().Result),
                    Task.Run(() => key2 = service2.GetCurrentKeyAsync().Result)
                },
                5 * 1000);
            
            Assert.NotNull(key1);
            Assert.NotNull(key2);
            Assert.Equal(key1, key2);
        }
    }

    public class KeyServiceFactoryData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new DefaultKeyServiceIntegrationFactory() };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
