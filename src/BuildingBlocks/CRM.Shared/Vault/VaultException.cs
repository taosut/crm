using System;
using CRM.Shared.Types;

namespace CRM.Shared.Vault
{
    public class VaultException : CRMException
    {
        public string Key { get; private set; }

        public VaultException(string key) : this(null, key)
        {

        }

        public VaultException(Exception inner, string key) : this(string.Empty, inner, key)
        {

        }

        public VaultException(string message, Exception inner, string key) : base(message)
        {
            Key = key;
        }
    }
}
