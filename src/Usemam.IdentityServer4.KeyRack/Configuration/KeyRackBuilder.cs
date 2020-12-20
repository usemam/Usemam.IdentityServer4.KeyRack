using Microsoft.Extensions.DependencyInjection;

namespace Usemam.IdentityServer4.KeyRack
{
    public class KeyRackBuilder
    {
        private readonly IIdentityServerBuilder _builder;

        public KeyRackBuilder(IIdentityServerBuilder builder)
        {
            _builder = builder;
        }

        public IServiceCollection Services => _builder.Services;

        public KeyRackBuilder AddPersistence<TRepository>()
            where TRepository : class, IKeyRepository
        {
            Services.AddTransient<IKeyRepository, TRepository>();
            return this;
        }

        public KeyRackBuilder AddFileSystemPersistence(string directoryPath)
        {
            Services.AddSingleton<IKeyRepository>(_ => new FileSystemKeyRepository(directoryPath));
            return this;
        }
    }
}