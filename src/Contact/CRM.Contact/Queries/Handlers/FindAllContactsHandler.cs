using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRM.Contact.Extensions;
using CRM.Protobuf.Contacts.V1;
using CRM.Shared.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CRM.Contact.Queries.Handlers
{
    public class FindAllContactsHandler : IRequestHandler<FindAllContactsQuery, ListContactsResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<FindAllContactsHandler> _logger;

        public FindAllContactsHandler(IUnitOfWork uow,
            ILogger<FindAllContactsHandler> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<ListContactsResponse> Handle(FindAllContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _uow.Connection.GetListAsync<Domain.Contact>();
            var response = new ListContactsResponse();

            _logger.LogInformation("Start query all contacts");
            response.Contacts.AddRange(contacts.Select(c => c.ToContactProtobuf()));
            return response;
        }
    }
}
