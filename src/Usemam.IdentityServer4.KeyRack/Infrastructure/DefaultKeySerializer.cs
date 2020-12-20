using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Usemam.IdentityServer4.KeyRack.Model;

namespace Usemam.IdentityServer4.KeyRack
{
    public class DefaultKeySerializer : IKeySerializer
    {
        private static JsonSerializerSettings _serializerSettings =
            new JsonSerializerSettings { ContractResolver = new GreedyContractResolver() };
        public RsaKey Deserialize(SerializedKey serializedKey) =>
            JsonConvert.DeserializeObject<RsaKey>(serializedKey.Data, _serializerSettings);

        public SerializedKey Serialize(RsaKey key) =>
            new SerializedKey(key, JsonConvert.SerializeObject(key, _serializerSettings));

        private class GreedyContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(
                MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.Ignored = false;
                return property;
            }
        }
    }
}