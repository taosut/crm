using CRM.Shared.Types;

namespace CRM.Shared.ValidationModel
{
    public class ValidationException : CRMException
    {
        public ValidationResultModel ValidationResultModel { get; private set; }
        public ValidationException(ValidationResultModel validationResultModel)
        {
            ValidationResultModel = validationResultModel;
        }
    }
}
