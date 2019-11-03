using CRM.Shared.Guard;

namespace CRM.Shared.CorrelationId
{
    public class CorrelationContext
    {
        internal CorrelationContext(string correlationId, string header)
        {
            correlationId.NotNullOrEmpty();
            header.NotNullOrEmpty();

            CorrelationId = correlationId;
            Header = header;
        }
        public string CorrelationId { get; }

        public string Header { get; }
    }
}
