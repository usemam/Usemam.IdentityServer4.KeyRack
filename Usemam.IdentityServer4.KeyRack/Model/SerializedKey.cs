namespace Usemam.IdentityServer4.KeyRack.Model
{
    public class SerializedKey : Key
    {
        public SerializedKey()
        {
        }

        public SerializedKey(Key key, string data)
            : base(key.KeyId, key.Created)
        {
            Data = data;
        }

        public string Data { get; set; }
    }
}