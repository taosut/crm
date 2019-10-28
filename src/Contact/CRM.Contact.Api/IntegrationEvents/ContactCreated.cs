using System;
using CRM.Shared.Types;
using MassTransit;

namespace CRM.IntegrationEvents
{
    public interface ContactCreated : IMessage
    {
        string FirstName { get; }
    }
}
