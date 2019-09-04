using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRM.Contact.Api.Domain;
using CRM.Contact.Contract.Queries;
using CRM.Contact.Contract.Queries.Dtos;
using MediatR;

namespace CRM.Contact.Api.Queries
{
    public class FindAllContactsHandler : IRequestHandler<FindAllContactsQuery, IEnumerable<ContactDto>>
    {
        private readonly IContactRepository _contactRepository;

        public FindAllContactsHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<IEnumerable<ContactDto>> Handle(FindAllContactsQuery request, CancellationToken cancellationToken)
        {
            var result = await _contactRepository.FindAllContacts();
            return result.Select(c => new ContactDto
            {
                ContactId = c.ContactId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MiddleName = c.MiddleName,
                Description = c.Description
            });
        }
    }
}