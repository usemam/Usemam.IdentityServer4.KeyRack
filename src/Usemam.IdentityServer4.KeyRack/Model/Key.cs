using System;

namespace Usemam.IdentityServer4.KeyRack.Model
{
    /// <summary>Key metadata base class</summary>
    public abstract class Key
    {
        /// <summary>Default constructor</summary>
        protected Key()
        {
        }

        /// <summary>Constructor with parameters</summary>
        /// <param name="keyId">Key identifier</param>
        /// <param name="created">Key created date</param>
        protected Key(string keyId, DateTime created)
        {
            KeyId = keyId;
            Created = created;
        }

        /// <summary>Key identifier</summary>
        public string KeyId { get; set; }

        /// <summary>Key created date</summary>
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