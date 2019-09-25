using System.Threading.Tasks;
using CRM.IntegrationEvents;
using CRM.Shared.EventBus;
using Microsoft.Extensions.Logging;

namespace CRM.Contact.IntegrationHandlers
{
    public class LeadCreatedEventHandler : IIntegrationEventHandler<LeadCreatedEvent>
    {
        private readonly ILogger<LeadCreatedEventHandler> _logger;
        public LeadCreatedEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LeadCreatedEventHandler>();
        }
        public async Task Handle(LeadCreatedEvent @event)
        {
            await Task.Delay(10);
            await Task.FromResult(0);
        }
    }
}
