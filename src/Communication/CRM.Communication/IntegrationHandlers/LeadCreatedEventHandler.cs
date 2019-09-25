using System.Threading.Tasks;
using CRM.Shared.EventBus;
using CRM.IntegrationEvents;

namespace CRM.Communication.IntegrationHandlers
{
    public class LeadCreatedEventHandler : IIntegrationEventHandler<LeadCreatedEvent>
    {
        public async Task Handle(LeadCreatedEvent @event)
        {
            await Task.Delay(10);
            await Task.FromResult(0);
        }
    }
}
