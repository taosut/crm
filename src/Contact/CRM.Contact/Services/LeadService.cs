using System.Threading.Tasks;
using CRM.Contact.Queries;
using CRM.Protobuf.Contacts.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Contact.Services
{
    public class LeadService : LeadApi.LeadApiBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LeadService> _logger;

        public LeadService(Shared.EventBus.Nats.INatsConnection connection,
            IMediator mediator,
            ILogger<LeadService> logger)
        {
            _mediator = mediator;

            _logger = logger;

            //connection.TryConnect();
        }

        public override async Task<PongReply> Ping(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("Ping handler happens ...");

            var result = await _mediator.Send(new PingQuery());
            return result;
        }
    }
}
