namespace Usemam.IdentityServer4.KeyRack.Model
{
    /// <summary>Serialized key container</summary>
    public class SerializedKey : Key
    {
        /// <summary>Default constructor</summary>
        public SerializedKey()
        {
        }

        /// <summary>Constructor with parameters</summary>
        /// <param name="key">Key metadata</param>
        /// <param name="data">Serialized key string</param>
        public SerializedKey(Key key, string data)
            : base(key.KeyId, key.Created)
        {
            Data = data;
        }

        public string Data { get; set; }
    }
}