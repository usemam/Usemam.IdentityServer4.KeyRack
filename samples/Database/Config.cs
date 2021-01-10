using System.Collections.Generic;

using IdentityServer4.Models;

namespace Database
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new ApiResource[] { };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[] {
                new ApiScope("scope", "Client scope")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[] {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "scope" }
                }
            };
        }
    }
}