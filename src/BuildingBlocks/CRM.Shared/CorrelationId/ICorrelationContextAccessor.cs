using System;

namespace CRM.Shared.CorrelationId
{
    public interface ICorrelationContextAccessor
    {
        CorrelationContext CorrelationContext { get; set; }
    }
}
