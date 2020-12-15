using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    public interface IKeySerializer
    {
        SerializedKey Serialize(RsaKey key);

        RsaKey Deserialize(SerializedKey serializedKey);
    }
}