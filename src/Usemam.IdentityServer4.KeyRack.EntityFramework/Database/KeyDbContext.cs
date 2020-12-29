using Microsoft.EntityFrameworkCore;

namespace Usemam.IdentityServer4.KeyRack.EntityFramework
{
    public class KeyDbContext : DbContext
    {
        private readonly DatabaseOptions _options;

        public KeyDbContext(DatabaseOptions options) => _options = options;

        public DbSet<Key> Keys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (_options?.DbContextConfigurationCallback != null)
            {
                _options.DbContextConfigurationCallback(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Key>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasMaxLength(200);
                entity.HasIndex(x => x.Name).IsUnique();
                entity.Property(x => x.Value).IsRequired();
            });
        }
    }
}