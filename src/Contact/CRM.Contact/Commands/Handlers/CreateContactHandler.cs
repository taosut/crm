using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using CRM.Shared.ValidationModel;
using CRM.Protobuf.Contacts.V1;
using CRM.Contact.Domain;
using CRM.Shared.Repository;
using CRM.Contact.Extensions;
using System;
using CRM.Shared.EventBus;
using CRM.IntegrationEvents;

namespace CRM.Contact.Commands.Handlers
{
    public class CreateContactHandler : IRequestHandler<CreateContactCommand, CreateContactResponse>
    {
        private readonly IValidator<CreateContactRequest> _validator;
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public CreateContactHandler(IValidator<CreateContactRequest> vadiator,
            IUnitOfWork uow, IEventBus eventBus)
        {
            _validator = vadiator;
            _uow = uow;
            _eventBus = eventBus;
        }

        public async Task<CreateContactResponse> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            await _validator.HandleValidation(request.ContactRequest);
            var requestedContact = request.ContactRequest;
            var contact = new Domain.Contact(ContactType.Contact,
                requestedContact.FirstName,
                requestedContact.LastName,
                requestedContact.Title,
                requestedContact.Company,
                requestedContact.Description,
                null);

            var contactId = await _uow.Connection.InsertAsync<Guid, Domain.Contact>(contact);

            _eventBus.Publish(new ContactCreatedEvent()
            {
                Id = Guid.NewGuid()
            });

            return new CreateContactResponse
            {
                Contact = contact.ToContactProtobuf()
            };
        }
    }
}
