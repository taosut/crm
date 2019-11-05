using System;
using System.Threading;

namespace CRM.Shared.CorrelationId
{
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        private static AsyncLocal<CorrelationContext> _correlationContext = new AsyncLocal<CorrelationContext>();        

        public CorrelationContext CorrelationContext
        {
            get => _correlationContext.Value;
            set => _correlationContext.Value = value;
        }
    }
}
