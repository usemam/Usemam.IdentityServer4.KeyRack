using Microsoft.AspNetCore.DataProtection;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack.DataProtection
{
    public class DataProtectionKeySerializer : IKeySerializer
    {
        private readonly IDataProtector _protector;

        public DataProtectionKeySerializer(IDataProtectionProvider protectionProvider)
        {
            _protector = protectionProvider.CreateProtector(nameof (DataProtectionKeySerializer));
        }

        public SerializedKey Serialize(RsaKey key)
        {
            string json = JsonSerializer.Serialize(key);
            return new SerializedKey(
                key,
                DataProtectionCommonExtensions.Protect(_protector, json));
        }

        public RsaKey Deserialize(SerializedKey serializedKey)
        {
            string json = DataProtectionCommonExtensions.Unprotect(_protector, serializedKey.Data);
            return JsonSerializer.Deserialize<RsaKey>(json);
        }
    }
}
