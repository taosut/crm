using CRM.Protobuf.Contact.V1;
using MediatR;

namespace CRM.Contact.Queries
{
    public class FindAllLeadsQuery : IRequest<ListContactsResponse>
    {

    }
}
