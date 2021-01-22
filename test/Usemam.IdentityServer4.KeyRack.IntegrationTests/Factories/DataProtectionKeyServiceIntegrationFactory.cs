using System;
using System.IO;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;

using Usemam.IdentityServer4.KeyRack.DataProtection;

namespace Usemam.IdentityServer4.KeyRack.IntegrationTests
{
    public class DataProtectionKeyServiceIntegrationFactory : IKeyServiceIntegrationFactory, IDisposable
    {
        private readonly IDataProtectionProvider _dataProtectionProvider =
            new EphemeralDataProtectionProvider();
        private readonly string _keysDirectoryPath = $".keys-{Guid.NewGuid()}";

        private ILoggerFactory _loggerFactory;

        public DataProtectionKeyServiceIntegrationFactory()
        {
            _loggerFactory = LoggerFactory.Create(builder => builder.SetMinimumLevel(LogLevel.Debug).AddConsole());
        }

        public IKeyService CreateService(KeyRackOptions options)
        {
            var timeKeeper = new TimeKeeper(options);
            var serializer = new DataProtectionKeySerializer(_dataProtectionProvider);
            var repository = new FileSystemKeyRepository(_keysDirectoryPath, _loggerFactory.CreateLogger<FileSystemKeyRepository>());
            return new KeyService(options, repository, serializer, timeKeeper, _loggerFactory.CreateLogger<KeyService>());
        }

        public void Dispose()
        {
            Directory.Delete(_keysDirectoryPath, true);

            _loggerFactory?.Dispose();
            _loggerFactory = null;
        }
    }
}