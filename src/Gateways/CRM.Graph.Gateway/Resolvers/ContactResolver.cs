using System;
using System.Collections.Generic;
using System.Linq;
using CRM.Graph.Gateway.Services;
using CRM.Protobuf.Contact.V1;
using HotChocolate.Resolvers;
using HotChocolate.Types.Relay;

namespace CRM.Graph.Gateway.Resolvers
{
    public class ContactResolver
    {
        private readonly IContactService _contactService;

        public ContactResolver(IContactService contactService)
        {
            _contactService = contactService;
        }

        public IList<Contact> ListContacts(IResolverContext context)
        {
            IDictionary<string, object> cursorProperties = context.GetCursorProperties();

            var contacts = _contactService.ListContacts().Contacts;

            var position = cursorProperties.TryGetValue("__position", out object p) ? Convert.ToInt32(p) : 0;
            var first = context.Argument<int>("first");
            var result = contacts.Skip(1).Take(9).ToList();
            //return contacts;
            return result;
        }
    }
}
