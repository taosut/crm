using System;
using System.Collections.Generic;

namespace CRM.Shared.EventBus
{
    public class IntegrationEvent
    {
        public Guid Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public Dictionary<string, string> TracingKeys { get; set; }
    }
}