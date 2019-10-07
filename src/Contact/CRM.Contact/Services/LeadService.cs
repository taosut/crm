using System.Threading.Tasks;
using CRM.Contact.Queries;
using CRM.Protobuf.Contacts.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;


namespace CRM.Contact.Services
{
    public class LeadService : LeadApi.LeadApiBase
    {
        private readonly IMediator _mediator;

        public LeadService(Shared.EventBus.Nats.INatsConnection connection,
            IMediator mediator)
        {
            _mediator = mediator;

            connection.TryConnect();
        }

        public override async Task<PongReply> Ping(Empty request, ServerCallContext context)
        {
            var result = await _mediator.Send(new PingQuery());
            return result;
        }
    }
}
