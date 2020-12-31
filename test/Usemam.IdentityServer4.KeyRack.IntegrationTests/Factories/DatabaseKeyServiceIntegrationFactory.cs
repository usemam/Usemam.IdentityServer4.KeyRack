using System;

using Microsoft.EntityFrameworkCore;

using Usemam.IdentityServer4.KeyRack.EntityFramework;

namespace Usemam.IdentityServer4.KeyRack.IntegrationTests
{
    public class DatabaseKeyServiceIntegrationFactory : IKeyServiceIntegrationFactory, IDisposable
    {
        private readonly DbContextOptions<KeyDbContext> _dbContextOptions;

        private readonly string _dbName = $"db-{Guid.NewGuid()}";

        private KeyDbContext _context;

        public DatabaseKeyServiceIntegrationFactory()
        {
            _dbContextOptions = new DbContextOptionsBuilder<KeyDbContext>().UseSqlite($"Filename={_dbName}.db").Options;
        }

        public IKeyService CreateService(KeyRackOptions options)
        {
            var timeKeeper = new TimeKeeper(options);
            var serializer = new DefaultKeySerializer();

            _context = new KeyDbContext(_dbContextOptions);
            _context.Database.EnsureCreated();
            var repository = new EntityFrameworkKeyRepository(_context);
            return new KeyService(options, repository, serializer, timeKeeper);
        }

        public void Dispose()
        {
            _context?.Database.EnsureDeleted();
            _context = null;
        }
    }
}