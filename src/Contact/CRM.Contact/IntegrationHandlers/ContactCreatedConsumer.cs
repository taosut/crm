using System.Threading.Tasks;
using CRM.IntegrationEvents;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Logging;

namespace CRM.Contact.IntegrationHandlers
{
    public class ContactCreatedConsumer : IConsumer<ContactCreated>
    {
        private readonly ILogger<ContactCreatedConsumer> _logger;

        public ContactCreatedConsumer(ILogger<ContactCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ContactCreated> context)
        {
            _logger.LogInformation("ContactCreatedConsumer - happened");
            await Task.Delay(10);
            await Task.FromResult(0);
        }
    }

    public class ContactCreatedConsumerDefinition : ConsumerDefinition<ContactCreatedConsumer>
    {
    }
}
