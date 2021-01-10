using System;
using System.Net.Http;
using System.Threading.Tasks;

using IdentityModel.Client;

namespace Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new HttpClient();

            // discover endpoints
            var discoveryDoc = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discoveryDoc.IsError)
            {
                Console.WriteLine(discoveryDoc.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDoc.TokenEndpoint,
                    ClientId = "client",
                    ClientSecret = "secret",
                    Scope = "scope"
                });
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
        }
    }
}
