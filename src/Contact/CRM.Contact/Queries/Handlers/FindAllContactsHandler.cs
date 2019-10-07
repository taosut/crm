using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRM.Contact.Extensions;
using CRM.Protobuf.Contacts.V1;
using CRM.Shared.Repository;
using MediatR;

namespace CRM.Contact.Queries.Handlers
{
    public class FindAllContactsHandler : IRequestHandler<FindAllContactsQuery, ListContactsResponse>
    {
        private readonly IUnitOfWork _uow;
        public FindAllContactsHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ListContactsResponse> Handle(FindAllContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _uow.Connection.GetListAsync<Domain.Contact>();
            var response = new ListContactsResponse();

            response.Contacts.AddRange(contacts.Select(c => c.ToContactProtobuf()));
            return response;
        }
    }
}
