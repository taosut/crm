using System.Threading.Tasks;
using CRM.Contact.Queries;
using CRM.Contact.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;


namespace CRM.Contact.Services
{
    public class LeadService : CRM.Contact.V1.LeadApi.LeadApiBase
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

        // [Authorize]
        // public override async Task<LeadsResponse> getLeads(Empty request, ServerCallContext context)
        // {
        //     var response = await _mediator.Send(new FindAllLeadsQuery());

        //     return response;
        // }

        // public override async Task<LeadInformation> createLead(CreateLeadRequest request, ServerCallContext context)
        // {
        //     var response = await _mediator.Send(new CreateLeadCommand());
        //     return response;
        // }
    }
}
