using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Usemam.IdentityServer4.KeyRack
{
    public static class JsonSerializer {
        private static JsonSerializerSettings _serializerSettings =
            new JsonSerializerSettings { ContractResolver = new GreedyContractResolver() };

        public static string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _serializerSettings);
        }

        public static T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, _serializerSettings);
        }

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