using System.Collections.Generic;
using CRM.Contact.Contract.Queries.Dtos;
using MediatR;

namespace CRM.Contact.Contract.Queries
{
    public class FindAllContactsQuery : IRequest<IEnumerable<ContactDto>>
    {
        
    }
}