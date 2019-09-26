using CRM.Protobuf.Contact.V1;
using FluentValidation;

namespace CRM.Contact.Validators
{
    public class CreateLeadRequestValidator : AbstractValidator<CreateContactRequest>
    {
        public CreateLeadRequestValidator()
        {
            RuleFor(x => x.Contact.LastName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Lead owner not be null or empty");
        }
    }
}
