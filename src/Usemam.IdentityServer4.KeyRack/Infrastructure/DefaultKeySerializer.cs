using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    public class DefaultKeySerializer : IKeySerializer
    {
        public RsaKey Deserialize(SerializedKey serializedKey) =>
            JsonSerializer.Deserialize<RsaKey>(serializedKey.Data);

        public SerializedKey Serialize(RsaKey key) =>
            new SerializedKey(key, JsonSerializer.Serialize(key));
    }
}