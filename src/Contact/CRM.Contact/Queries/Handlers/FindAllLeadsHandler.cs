using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRM.Contact.Domain;
using CRM.Protobuf.Contact.V1;
using MediatR;

namespace CRM.Contact.Queries.Handlers
{
    public class FindAllLeadsHandler : IRequestHandler<FindAllLeadsQuery, ListContactsResponse>
    {
        private IContactRepository _contactRepo;
        public FindAllLeadsHandler(IContactRepository contactRepo)
        {
            _contactRepo = contactRepo;
        }

        public async Task<ListContactsResponse> Handle(FindAllLeadsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _contactRepo.ListContactsAsync();

            var response = new ListContactsResponse();
            response.Contacts.AddRange(contacts.Select(c => new Protobuf.Contact.V1.Contact
            {
                Id = c.ContactId.ToString(),
                FirstName = c.FirstName,
                LastName = c.LastName,
                Company = c.Company,
                Description = c.Description,
                Title = c.Title,
                Address = new Protobuf.Contact.V1.Address
                {
                    City = c.Address.MailingCity,
                    Country = c.Address.MailingCountry,
                    State = c.Address.MailingState,
                    Street = c.Address.MailingStreet,
                    Zipcode = c.Address.MailingZipCode
                },
                ContactInfo = new Protobuf.Contact.V1.ContactInformation
                {
                    Email = c.ContactInfo.Email,
                    HomePhone = c.ContactInfo.HomePhone,
                    Mobile = c.ContactInfo.Mobile,
                    WorkPhone = c.ContactInfo.WorkPhone
                }
            }));

            return response;
        }
    }
}
