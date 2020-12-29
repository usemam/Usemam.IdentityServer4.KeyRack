using System;

using Microsoft.EntityFrameworkCore;

namespace Usemam.IdentityServer4.KeyRack.EntityFramework
{
    public class DatabaseOptions
    {
        public Action<DbContextOptionsBuilder> DbContextConfigurationCallback { get; set; }
    }
}