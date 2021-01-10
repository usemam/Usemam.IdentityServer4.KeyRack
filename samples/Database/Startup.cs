using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Usemam.IdentityServer4.KeyRack.EntityFramework;

namespace Database
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration config, IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDataProtection();

            services
                .AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddKeyManagement(
                    options =>
                    {
                        // default key management options are modified for testing purpose
                        options.KeyActivation = TimeSpan.FromSeconds(10);
                        options.KeyExpiration = options.KeyActivation * 2;
                        options.KeyRetirement = options.KeyActivation * 3;
                    })
                .AddDatabasePersistence(new DatabaseOptions
                {
                    DbContextConfigurationCallback = b =>
                        b.UseSqlite("Filename=keyStore.db")
                })
                .AddDataProtection();
        }

        public void Configure(IApplicationBuilder app)
        {
            InitializeDatabase(app);

            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<KeyDbContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
