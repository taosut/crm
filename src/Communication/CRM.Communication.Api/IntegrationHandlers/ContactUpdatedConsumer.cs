using System.Threading.Tasks;
using CRM.IntegrationEvents;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Logging;

namespace CRM.Communication.IntegrationHandlers
{
    public class ContactUpdatedConsumer : IConsumer<ContactUpdated>
    {
        private readonly ILogger<ContactUpdatedConsumer> _logger;
        public ContactUpdatedConsumer(ILogger<ContactUpdatedConsumer> logger)
        {
            _logger = logger;

        }
        public async Task Consume(ConsumeContext<ContactUpdated> context)
        {
            _logger.LogInformation("ContactUpdatedConsumer - happens");

            await Task.Delay(10);
            await Task.FromResult(0);
        }
    }

    public class ContactUpdatedConsumerDefinition : ConsumerDefinition<ContactUpdatedConsumer>
    {
    }
}
