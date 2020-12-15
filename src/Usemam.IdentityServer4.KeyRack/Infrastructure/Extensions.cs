using System.Security.Cryptography;

using IdentityModel;

using Microsoft.IdentityModel.Tokens;

namespace Usemam.IdentityServer4.KeyRack
{
    public static class Extensions
    {
        public static RsaSecurityKey CreateSecurityKey(this KeyRackOptions options)
        {
            var rsa = RSA.Create();
            RsaSecurityKey securityKey = null;
            if (rsa is RSACryptoServiceProvider)
            {
                // Windows OS - use RSACng instead of RSACryptoServiceProvider
                rsa.Dispose();
                securityKey = new RsaSecurityKey(new RSACng(options.KeySize).ExportParameters(includePrivateParameters: true));
            }
            else
            {
                rsa.KeySize = options.KeySize;
                securityKey = new RsaSecurityKey(rsa);
            }

            securityKey.KeyId = CryptoRandom.CreateUniqueId(options.KeyIdSize / 8, CryptoRandom.OutputFormat.Hex);
            return securityKey;
        }
    }
}