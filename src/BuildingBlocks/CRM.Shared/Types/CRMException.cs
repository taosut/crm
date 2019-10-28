using System;

namespace CRM.Shared.Types
{
    public class CRMException : Exception
    {
        public string Code { get; }

        public CRMException()
        {
        }

        public CRMException(string code)
        {
            Code = code;
        }

        public CRMException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        public CRMException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        public CRMException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public CRMException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
