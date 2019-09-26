using CRM.Protobuf.Contact.V1;
using MediatR;

namespace CRM.Contact.Commands
{
    public class CreateContactCommand : IRequest<CreateContactRequest>
    {
        public CreateContactRequest LeadRequest { get; set; }
    }
}
