using System.Collections.Generic;
using CRM.Graph.Gateway.Services;

namespace CRM.Graph.Gateway.Resolvers
{
    public class ContactResolver
    {
        private readonly IContactService _contactService;

        public ContactResolver(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IList<CRM.Protobuf.Contact.V1.Contact> ListContacts()
        {
            var contacts = _contactService.ListContacts().Contacts;
            return contacts;
        }
    }
}
