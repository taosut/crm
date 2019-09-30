using CRM.Protobuf.Contact.V1;
using CRM.Shared.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Logging;
using static CRM.Protobuf.Contact.V1.ContactApi;
using static CRM.Protobuf.Contact.V1.LeadApi;

namespace CRM.Graph.Gateway.Services
{
    public class ContactService : ServiceBase, IContactService
    {
        private readonly ContactApiClient _contactClient;
        private readonly LeadApiClient _leadClient;
        private readonly ILogger<ContactService> _logger;

        public ContactService(GrpcClientFactory clientFactory, ILoggerFactory loggerFactory) : base()
        {
            _contactClient = clientFactory.CreateClient<ContactApiClient>(nameof(ContactApiClient));
            _leadClient = clientFactory.CreateClient<LeadApiClient>(nameof(LeadApiClient));
            _logger = loggerFactory.CreateLogger<ContactService>();
        }
        public ListContactsResponse ListContacts()
        {
            var metaData = new Metadata();

            _leadClient.Ping(new Empty());
            var result = _contactClient.ListContacts(new Empty(), metaData);
            _logger.LogInformation(result.Contacts.ToString());
            return result;
        }
    }
}
