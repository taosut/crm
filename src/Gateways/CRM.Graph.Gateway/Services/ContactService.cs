using CRM.Protobuf.Contacts.V1;
using CRM.Shared.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Logging;
using static CRM.Protobuf.Contacts.V1.ContactApi;

namespace CRM.Graph.Gateway.Services
{
    public class ContactService : ServiceBase, IContactService
    {
        private readonly ContactApiClient _contactClient;
        private readonly ILogger<ContactService> _logger;

        public ContactService(GrpcClientFactory clientFactory, ILoggerFactory loggerFactory) : base()
        {
            _contactClient = clientFactory.CreateClient<ContactApiClient>(nameof(ContactApiClient));
            _logger = loggerFactory.CreateLogger<ContactService>();
        }

        public CreateContactResponse CreateContact(CreateContactRequest contact)
        {
            var result = _contactClient.CreateContact(contact);
            return result;
        }

        public ListContactsResponse ListContacts()
        {
            var metaData = new Metadata();
            var result = _contactClient.ListContacts(new Empty(), metaData);
            _logger.LogInformation(result.Contacts.ToString());
            return result;
        }
    }
}
