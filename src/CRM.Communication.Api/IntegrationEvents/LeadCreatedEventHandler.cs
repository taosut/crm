using System.Threading.Tasks;
using CRM.DataContract.IntegrationEvents.Lead;
using CRM.Shared.EventBus;

namespace CRM.Communication.Api.IntegrationEvents
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