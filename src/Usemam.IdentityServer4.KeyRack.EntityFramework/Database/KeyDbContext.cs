using Microsoft.EntityFrameworkCore;

namespace Usemam.IdentityServer4.KeyRack.EntityFramework
{
    public class KeyDbContext : DbContext
    {
        public KeyDbContext(DbContextOptions<KeyDbContext> options)
            : base(options) { }

        public DbSet<Key> Keys { get; set; }

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