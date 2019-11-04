using System;

namespace CRM.Shared.Types
{
    public interface IMessage
    {
        Guid Id { get; }
        Guid CorrelationId { get; }
        DateTime? CreatedDate { get; }

    }
}
