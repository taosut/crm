using System.Threading.Tasks;
using CRM.Contact.Commands;
using CRM.Contact.Queries;
using CRM.Protobuf.Contacts.V1;
using Google.Protobuf.WellKnownTypes;
using MediatR;

namespace CRM.Contact.Services
{
    public class ContactService : ContactApi.ContactApiBase
    {
        private readonly IMediator _mediator;

        public ContactService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<CreateContactResponse> CreateContact(CreateContactRequest contactRequest, Grpc.Core.ServerCallContext context)
        {
            return await _mediator.Send(new CreateContactCommand(contactRequest));

        }

        public override async Task<ListContactsResponse> ListContacts(Empty request, Grpc.Core.ServerCallContext context)
        {
            return await _mediator.Send(new FindAllContactsQuery());
        }

        public override async Task<CRM.Protobuf.Contacts.V1.Contact> GetContact(GetContactRequest request, Grpc.Core.ServerCallContext context)
        {
            return await _mediator.Send(new FindContactByIdQuery(new System.Guid(request.ContactId)));
        }
    }
}
