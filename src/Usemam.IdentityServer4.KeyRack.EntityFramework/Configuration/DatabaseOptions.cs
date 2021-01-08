using System;

using Microsoft.EntityFrameworkCore;

namespace Usemam.IdentityServer4.KeyRack.EntityFramework
{
    /// <summary>Database access options</summary>
    public class DatabaseOptions
    {
        /// <summary>Required. <see cref="DbContextOptionsBuilder" /> configuration callback</summary>
        public Action<DbContextOptionsBuilder> DbContextConfigurationCallback { get; set; }
    }
}