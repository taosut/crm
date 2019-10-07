using System.Threading.Tasks;
using CRM.IntegrationEvents;
using CRM.Shared.EventBus;
using Microsoft.Extensions.Logging;

namespace CRM.Contact.IntegrationHandlers
{
    public class ContactCreatedEventHandler : IIntegrationEventHandler<ContactCreatedEvent>
    {
        private readonly ILogger<ContactCreatedEventHandler> _logger;
        public ContactCreatedEventHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ContactCreatedEventHandler>();
        }
        public async Task Handle(ContactCreatedEvent @event)
        {
            await Task.Delay(10);
            await Task.FromResult(0);
        }
    }
}
