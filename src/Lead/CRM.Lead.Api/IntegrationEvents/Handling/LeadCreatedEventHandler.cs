using System.Threading.Tasks;
using CRM.DataContract.IntegrationEvents.Lead;
using CRM.Shared.EventBus;
using Microsoft.Extensions.Logging;

namespace CRM.Lead.Api.IntegrationEvents.Handling
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