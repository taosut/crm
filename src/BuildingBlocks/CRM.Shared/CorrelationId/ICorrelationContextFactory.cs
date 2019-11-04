using System;

namespace CRM.Shared.CorrelationId
{
    public interface ICorrelationContextFactory
    {
        CorrelationContext Create(Guid correlationId, String header);
        void Dispose();
    }
}
