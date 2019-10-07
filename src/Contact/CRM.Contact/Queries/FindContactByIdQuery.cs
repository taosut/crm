using System;
using MediatR;

namespace CRM.Contact.Queries
{
    public class FindContactByIdQuery : IRequest<CRM.Protobuf.Contacts.V1.Contact>
    {
        public FindContactByIdQuery(Guid contactId)
        {
            this.ContactId = contactId;

        }
        public Guid ContactId { get; private set; }

    }
}
