using CRM.Protobuf.Contacts.V1;
using MediatR;

namespace CRM.Contact.Commands
{
    public class CreateContactCommand : IRequest<CreateContactResponse>
    {
        public CreateContactCommand(CreateContactRequest contactRequest)
        {
            this.ContactRequest = contactRequest;

        }
        public CreateContactRequest ContactRequest { get; private set; }
    }
}
