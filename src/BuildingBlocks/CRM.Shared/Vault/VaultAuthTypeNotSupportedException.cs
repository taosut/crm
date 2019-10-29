using System;
using CRM.Shared.Types;

namespace CRM.Shared.Vault
{
    public class VaultAuthTypeNotSupportedException : CRMException
    {
        public string AuthType { get; private set; }

        public VaultAuthTypeNotSupportedException(string authType) : this(string.Empty, authType)
        {

        }

        public VaultAuthTypeNotSupportedException(string message, string authType) : base(message)
        {
            AuthType = authType;
        }
    }
}
