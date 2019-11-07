using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRM.Protobuf.Contacts.V1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Logging;
using static CRM.Protobuf.Contacts.V1.ContactApi;

namespace CRM.Graph.Gateway.Types.Contacts
{
    public class ContactResolver
    {
        private readonly ContactApiClient _contactClient;
        private readonly ILogger<ContactResolver> _logger;

        public ContactResolver(GrpcClientFactory clientFactory, ILoggerFactory loggerFactory)
        {
            _contactClient = clientFactory.CreateClient<ContactApiClient>(nameof(ContactApiClient));
            _logger = loggerFactory.CreateLogger<ContactResolver>();
        }

        public async Task<IList<Contact>> ListContacts()
        {
            var metaData = new Metadata();
            var result = await _contactClient.ListContactsAsync(new Empty(), metaData);
            return result.Contacts;
        }

        public Contact GetContactById(Guid contactId)
        {
            var metaData = new Metadata();
            var result = _contactClient.GetContact(new GetContactRequest { ContactId = contactId.ToString() }, metaData);
            return result;
        }

        public async Task<Contact> CreateNewContact(CreateContactRequest contact)
        {
            _logger.LogInformation("ContactResolver - CreateNewContact");
            var metaData = new Metadata();
            var result = await _contactClient.CreateContactAsync(contact, metaData);
            return result.Contact;
        }

        public async Task<Boolean> UploadPhoto(UploadPhotoRequest photo)
        {
            _logger.LogInformation("ContactResolver - UploadPhoto");
            var metaData = new Metadata();
            var result = await _contactClient.UploadPhotoAsync(photo, metaData);
            return result.Status;
        }
    }
}
