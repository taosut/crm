using System.Threading.Tasks;
using CRM.Contact.Queries;
using CRM.IntegrationEvents;
using CRM.Protobuf.Contacts.V1;
using CRM.Shared.CorrelationId;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Contact.Services
{
    public class LeadService : LeadApi.LeadApiBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LeadService> _logger;
        private readonly IBusControl _bus;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public LeadService(IMediator mediator, ILogger<LeadService> logger,
            IBusControl bus, ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
            _mediator = mediator;
            _logger = logger;
            _bus = bus;
        }

        public override async Task<PongReply> Ping(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Ping handler happens ...");
            await _bus.Publish<ContactCreated>(new
            {
                FirstName = "sss",
                CorrelationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId
            });
            var result = await _mediator.Send(new PingQuery());
            return result;
        }
    }
}
