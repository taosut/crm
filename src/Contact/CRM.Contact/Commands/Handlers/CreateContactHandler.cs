using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using CRM.Shared.ValidationModel;
using CRM.Contact.V1;

namespace CRM.Contact.Commands.Handlers
{
    public class CreateContactHandler : IRequestHandler<CreateContactCommand, CreateContactRequest>
    {
        private readonly IValidator<CreateContactRequest> _validator;

        public CreateContactHandler(IValidator<CreateContactRequest> vadiator)
        {
            _validator = vadiator;
        }

        public async Task<CreateContactRequest> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            await _validator.HandleValidation(request.LeadRequest);

            return null;
        }
    }
}
