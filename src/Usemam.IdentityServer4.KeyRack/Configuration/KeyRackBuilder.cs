using Microsoft.Extensions.DependencyInjection;

namespace Usemam.IdentityServer4.KeyRack
{
    /// <summary>Key management configuration builder</summary>
    public class KeyRackBuilder
    {
        private readonly IIdentityServerBuilder _builder;

        public KeyRackBuilder(IIdentityServerBuilder builder)
        {
            _builder = builder;
        }

        public IServiceCollection Services => _builder.Services;

        /// <summary>Add custom key persistence implementation</summary>
        public KeyRackBuilder AddPersistence<TRepository>()
            where TRepository : class, IKeyRepository
        {
            Services.AddTransient<IKeyRepository, TRepository>();
            return this;
        }

        /// <summary>Persist keys to file system</summary>
        public KeyRackBuilder AddFileSystemPersistence(string directoryPath)
        {
            Services.AddSingleton<IKeyRepository>(_ => new FileSystemKeyRepository(directoryPath));
            return this;
        }
    }
}