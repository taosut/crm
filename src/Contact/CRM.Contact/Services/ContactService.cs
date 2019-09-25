using System.Threading.Tasks;
using CRM.Contact.Queries;
using CRM.Contact.V1;
using Google.Protobuf.WellKnownTypes;
using MediatR;

namespace CRM.Contact.Services
{
    public class ContactService : CRM.Contact.V1.ContactApi.ContactApiBase
    {
        private readonly IMediator _mediator;

        public ContactService(Shared.EventBus.Nats.INatsConnection connection,
            IMediator mediator)
        {
            _mediator = mediator;

            connection.TryConnect();
        }

        public override Task<CreateContactResponse> CreateContact(CreateContactRequest request, Grpc.Core.ServerCallContext context)
        {
            return null;
        }

        public override async Task<ListContactsResponse> ListContacts(Empty request, Grpc.Core.ServerCallContext context)
        {
            return await _mediator.Send(new FindAllLeadsQuery());
        }
    }
}
