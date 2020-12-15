using System;

namespace Usemam.IdentityServer4.KeyRack.Model
{
    public abstract class Key
    {
        protected Key()
        {
        }

        protected Key(string keyId, DateTime created)
        {
            KeyId = keyId;
            Created = created;
        }

        public string KeyId { get; set; }

        public DateTime Created { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var that = (Key)obj;
            return this.KeyId == that.KeyId;
        }

        public override int GetHashCode()
        {
            return KeyId.GetHashCode();
        }
    }
}