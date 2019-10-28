using System;

namespace CRM.Shared.Types
{
    public interface IMessage
    {
        Guid Id { get; }
        DateTime? CreatedDate { get; }

    }
}
