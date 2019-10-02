using CRM.Protobuf.Contacts.V1;
using FluentValidation;

namespace CRM.Contact.Validators
{
    public class CreateContactRequestValidator : AbstractValidator<CreateContactRequest>
    {
        public CreateContactRequestValidator()
        {
            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty()
                .WithMessage("The last name could not be null or empty");

            RuleFor(x => x.Company)
                .NotNull()
                .NotEmpty()
                .WithMessage("The company could not be null or empty");
        }
    }
}
